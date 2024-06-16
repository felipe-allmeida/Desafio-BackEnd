using BuildingBlocks.Common;
using BuildingBlocks.Identity.Jwt;
using BuildingBlocks.Identity.Models;
using BikeRental.API.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text.Json;
using BikeRental.Application.Extensions;
using BikeRental.Domain.Exceptions;
using BikeRental.API.Services;

namespace BikeRental.API.Controllers.V1
{

    [ApiController]
    [Authorize(Policy = Policies.NotAnonymous)]
    [Route("api/v1/accounts")]
    public class AccountsController(
        IMediator mediator,
        ILoggedUserService loggedUser,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager,
        IOptions<AppJwtOptions> appJwtSettings) : ControllerBase
    {
        private readonly ILoggedUserService _loggedUser = loggedUser ?? throw new ArgumentNullException(nameof(loggedUser));
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        private readonly UserManager<IdentityUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly RoleManager<IdentityRole> _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        private readonly SignInManager<IdentityUser> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly AppJwtOptions _appJwtSettings = appJwtSettings.Value ?? throw new ArgumentNullException(nameof(appJwtSettings));


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] UserRegister register)
        {
            IdentityUser? identityUser;

            if (_loggedUser.IsAuthenticated)
            {
                identityUser = await _userManager.FindByEmailAsync(_loggedUser.IdentityUserEmail.ToUpperInvariant());
                if (identityUser is null) throw new NotFoundException();

                await _userManager.AddPasswordAsync(identityUser, register.Password);

                return CreatedAtAction(nameof(Post), new { id = identityUser.Id }, new { id = identityUser.Id });
            }
            else
            {
                identityUser = new IdentityUser
                {
                    UserName = register.Email,
                    Email = register.Email,
                    EmailConfirmed = true,
                };

                var result = await _userManager.CreateAsync(identityUser, register.Password);

                if (!result.Succeeded) throw new ConflictException(string.Join(';', result.Errors.Select(x => $"{x.Code}: {x.Description}")));


                return CreatedAtAction(nameof(Post), new { id = identityUser.Id }, new { id = identityUser.Id });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var identityUser = await _userManager.FindByEmailAsync(_loggedUser.IdentityUserName);
            if (identityUser is null) throw new NotFoundException();

            var result = await _userManager.DeleteAsync(identityUser);
            if (!result.Succeeded) throw new ConflictException(string.Join(';', result.Errors.Select(x => $"{x.Code}: {x.Description}")));

            return NoContent();
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLogin login)
        {
            var identityUser = await _userManager.FindByEmailAsync(login.Email.ToUpperInvariant());

            if (identityUser is null) return Unauthorized();
            var result = await _signInManager.PasswordSignInAsync(identityUser, login.Password, false, true);

            if (result.IsLockedOut)
            {
                return Forbid();
            }

            if (!result.Succeeded)
            {
                if (!await _userManager.IsEmailConfirmedAsync(identityUser))
                {
                    var problemDetails = new ValidationProblemDetails()
                    {
                        Instance = HttpContext.Request.Path,
                        Status = StatusCodes.Status401Unauthorized,
                        Detail = "E-mail is not confirmed."
                    };
                    return Unauthorized(problemDetails);
                }

                return Unauthorized();
            }

            return await GenerateJwtAccessToken(identityUser!);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] Token token)
        {
            var result = await new JwtValidator(_appJwtSettings).ValidateToken(token.RefreshToken);

            if (!result.IsValid)
            {
                return Unauthorized();
            }

            var email = result.Claims[JwtRegisteredClaimNames.Email].ToString();
            var identityUser = await _userManager.FindByEmailAsync(email.ToUpperInvariant());

            var claims = await _userManager.GetClaimsAsync(identityUser);

            if (!claims.Any(c => c.Type == "LastRefreshToken" && c.Value == result.Claims[JwtRegisteredClaimNames.Jti].ToString()))
                return Unauthorized();

            if (identityUser.LockoutEnabled && identityUser.LockoutEnd < DateTime.Now)
                return Forbid();


            var jwtBuilder = new JwtBuilder<IdentityUser, IdentityRole>(_userManager, _roleManager, _appJwtSettings, email!);

            return Ok(await jwtBuilder.GenerateAccessAndRefreshToken());
        }


        private async Task<IActionResult> GenerateJwtAccessToken(IdentityUser identityUser)
        {
            var jwtBuilder = new JwtBuilder<IdentityUser, IdentityRole>(_userManager, _roleManager, _appJwtSettings, identityUser.Email!);

            return Ok(await jwtBuilder.GenerateAccessAndRefreshToken());
        }
    }
}

using BikeRental.API.DTOs.V1.Requests;
using BikeRental.API.Infrastructure.Security;
using BikeRental.Application.Commands.V1.User.RentBike;
using BikeRental.Application.Commands.V1.User.UpdateRentStatus;
using BikeRental.Application.DTOs.V1;
using BikeRental.Application.Queries.V1.User.GetRentals;
using BikeRental.Application.Queries.V1.User.GetRentBikeInfo;
using BikeRental.Domain.Enums;
using BuildingBlocks.Common;
using BuildingBlocks.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.API.Areas.User.Controllers
{
    [Area("user")]
    [ApiController]
    [Route("api/v1/[area]/rentals")]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly LinkGenerator _linkGenerator;

        public RentalsController(IMediator mediator, IIdentityService identityService, LinkGenerator linkGenerator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
        }

        [HttpGet("info")]
        [Authorize(Policy = Policies.DeliveryRiderRead)]
        public async Task<ActionResult<RentBikeDto>> GetRentalInfo([FromQuery] GetRentBikeInfoQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = Policies.DeliveryRiderRead)]
        public async Task<ActionResult<PaginatedItem<RentalDto>>> GetRentals([FromQuery] GetRentalsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = Policies.DeliveryRiderWrite)]
        public async Task<IActionResult> CreateRental([FromBody] RentBikeDto body)
        {
            var result = await _mediator.Send(new RentBikeCommand
            {
                UserId = _identityService.GetUserId(),
                StartAt = body.StartAt,
                EndAt = body.EndAt,
                ExpectedReturnAt = body.ExpectedReturnAt
            });

            return CreatedAtAction(nameof(CreateRental), new { id = result.Id }, new { id = result.Id });
        }

        [HttpPatch("{id}/status")]
        [Authorize(Policy = Policies.DeliveryRiderWrite)]
        public async Task<IActionResult> UpdateRentalStatus([FromRoute] Guid id, [FromBody] UpdateRentalDto body)
        {
            await _mediator.Send(new UpdateRentStatusCommand
            {
                UserId = _identityService.GetUserId(),
                RentalId = id,
                Status = body.Status
            });

            return NoContent();
        }
    }
}

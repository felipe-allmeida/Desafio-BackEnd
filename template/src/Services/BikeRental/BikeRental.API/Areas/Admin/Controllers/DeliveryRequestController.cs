using BikeRental.API.DTOs.V1.Requests;
using BikeRental.API.Infrastructure.Security;
using BikeRental.Application.Commands.V1.Admin.CreateDeliveryRequest;
using BuildingBlocks.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.API.Areas.Admin.Controllers
{
    [Area("admin")]
    [ApiController]
    [Route("api/v1/[area]/delivery-requests")]
    public class DeliveryRequestController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public DeliveryRequestController(IMediator mediator, IIdentityService identityService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [HttpPost]
        [Authorize(Policy = Policies.AdminWrite)]
        public async Task<IActionResult> CreateDeliveryRequest([FromBody] CreateDeliveryRequestDto body)
        {
            var result = await _mediator.Send(new CreateDeliveryRequestCommand
            {
                PriceCents = body.PriceCents
            });

            return CreatedAtAction(nameof(CreateDeliveryRequest), new { id = result.Id }, new { id = result.Id });
        }
    }
}

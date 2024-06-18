using BikeRental.API.DTOs.V1.Requests;
using BikeRental.API.Infrastructure.Security;
using BikeRental.Application.Commands.V1.User.AcceptDeliveryRequest;
using BikeRental.Application.Commands.V1.User.CompleteDeliveryRequest;
using BikeRental.Application.Commands.V1.User.CreateDeliveryRider;
using BikeRental.Application.Commands.V1.User.UpdateDeliveryRiderCnh;
using BikeRental.Application.DTOs.V1;
using BikeRental.Application.Queries.V1.User.GetDeliveryRider;
using BuildingBlocks.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.API.Areas.User.Controllers
{
    [Area("user")]
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

        [HttpPost("{id}/accept")]
        [Authorize(Policy = Policies.DeliveryRiderWrite)]
        public async Task<IActionResult> AcceptDeliveryRequest([FromRoute] Guid id)
        {
            await _mediator.Send(new AcceptDeliveryRequestCommand
            {
                DeliveryRequestId = id,
                UserId = _identityService.GetUserId()
            });

            return NoContent();
        }

        [HttpPost("{id}/finish")]
        [Authorize(Policy = Policies.DeliveryRiderWrite)]
        public async Task<IActionResult> CompleteDeliveryRequest([FromRoute] Guid id)
        {
            await _mediator.Send(new CompleteDeliveryRequestCommand
            {
                UserId = _identityService.GetUserId()
            });

            return NoContent();
        }

    }

    [Area("user")]
    [ApiController]
    [Route("api/v1/[area]/delivery-riders")]
    public class DeliveryRidersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly LinkGenerator _linkGenerator;

        public DeliveryRidersController(IMediator mediator, IIdentityService identityService, LinkGenerator linkGenerator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
        }

        [HttpGet("me")]
        [Authorize(Policy = Policies.DeliveryRiderRead)]
        public async Task<ActionResult<DeliveryRiderDto>> GetDeliveryRider()
        {
            var result = await _mediator.Send(new GetDeliveryRiderQuery { UserId = _identityService.GetUserId() });

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = Policies.DeliveryRiderWrite)]
        public async Task<IActionResult> CreateDeliveryRider([FromBody] CreateDeliveryRiderDto body)
        {
            var result = await _mediator.Send(new CreateDeliveryRiderCommand
            {
                UserId = _identityService.GetUserId(),
                Name = body.Name,
                Cnpj = body.Cnpj,
                Birthday = body.Birthday
            });

            return CreatedAtAction(nameof(CreateDeliveryRider), new { id = result.Id }, new { id = result.Id });
        }

        [HttpPatch("me/cnh")]
        [Authorize(Policy = Policies.DeliveryRiderWrite)]
        public async Task<IActionResult> UpdateCnh([FromForm] UpdateDeliveryRiderCnhDto body)
        {
            await _mediator.Send(new UpdateDeliveryRiderCnhCommand
            {
                UserId = _identityService.GetUserId(),
                CnhType = body.Type,
                CnhNumber = body.Number,
                CnhImage = body.File
            });

            return NoContent();
        }


    }
}

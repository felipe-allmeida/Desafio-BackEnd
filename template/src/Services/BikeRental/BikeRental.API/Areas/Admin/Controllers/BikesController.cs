using BikeRental.API.DTOs.V1.Requests;
using BikeRental.API.Infrastructure.Security;
using BikeRental.Application.Commands.V1.Admin.CreateBike;
using BikeRental.Application.Commands.V1.Admin.RemoveBike;
using BikeRental.Application.Commands.V1.Admin.UpdateBikePlate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BikeRental.CrossCutting.Storage.Abstractions;
using BuildingBlocks.Common;
using BikeRental.Application.DTOs.V1;
using BikeRental.Application.Queries.V1.Admin.GetBikes;
using BikeRental.Application.Queries.V1.Admin.GetBike;

namespace BikeRental.API.Areas.Admin.Controllers
{
    [Area("admin")]
    [ApiController]
    [Route("api/v1/[area]/bikes")]
    public class BikesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly LinkGenerator _linkGenerator;

        public BikesController(IMediator mediator, LinkGenerator linkGenerator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
        }

        [HttpGet]
        [Authorize(Policy = Policies.AdminRead)]
        public async Task<ActionResult<PaginatedItem<BikeDto>>> GetBikes([FromQuery] GetBikesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        [Authorize(Policy = Policies.AdminRead)]
        public async Task<ActionResult<BikeDto>> GetBike([FromRoute] long id)
        {
            var result = await _mediator.Send(new GetBikeQuery { Id = id });

            if (result is null) 
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AdminWrite)]
        public async Task<IActionResult> CreateBike([FromBody] CreateBikeDto body)
        {
            var result = await _mediator.Send(new CreateBikeCommand
            {
                Plate = body.Plate,
                Model = body.Model,
                Year = body.Year,
            });
            return CreatedAtAction(nameof(CreateBike), new { id = result.Id }, new { id = result.Id });
        }

        [HttpPatch("{id:long}/plate")]
        [Authorize(Policy = Policies.AdminWrite)]
        public async Task<IActionResult> UpdateBikePlate([FromRoute] long id, [FromBody] UpdateBikePlateDto body)
        {
            await _mediator.Send(new UpdateBikePlateCommand
            {
                Id = id,
                Plate = body.Plate,
            });
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        [Authorize(Policy = Policies.AdminWrite)]
        public async Task<IActionResult> DeleteBike([FromRoute] long id)
        {
            await _mediator.Send(new RemoveBikeCommand
            {
                Id = id
            });
            return NoContent();
        }
    }
}

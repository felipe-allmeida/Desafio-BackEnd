using BikeRental.API.DTOs.V1.Requests;
using BikeRental.API.Infrastructure.Security;
using BikeRental.Application.Commands.V1.Admin.CreateBike;
using BikeRental.Application.Commands.V1.Admin.RemoveBike;
using BikeRental.Application.Commands.V1.Admin.UpdateBikePlate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BikeRental.CrossCutting.Storage.Abstractions;

namespace BikeRental.API.Areas.Admin.Controllers
{
    [Area("admin")]
    [ApiController]
    [Route("api/v1/[area]/bikes")]
    public class BikeController(IMediator mediator, LinkGenerator linkGenerator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        private readonly LinkGenerator _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));

        //http(s)://<minio-server-endpoint>:<port>/<bucket-name>/<object-key>
        //http://localhost:9000/test/cnh/6/15/2024%209:50:01%E2%80%AFPM%20+00:00.jpeg
        [HttpPost("xablau")]
        [AllowAnonymous]
        public async Task<IActionResult> Xablau([FromForm] FileDto file, [FromServices] IStorageService storageService)
        {
            using var stream = file.File.OpenReadStream();

            var fileName = $"documents/cnh{Path.GetExtension(file.File.FileName)}";
            var response =await storageService.UploadBlob("test", fileName, stream, file.File.ContentType);

            var asd = await storageService.GetBlobAsync("test", fileName);

            return Ok(asd);
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

        [HttpPatch("{id:long}")]
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

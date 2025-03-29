using MediatR;
using Microsoft.AspNetCore.Mvc;
using Upload.API.Application.Models;
using Upload.API.Application.UseCases.Upload;

namespace Upload.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UploadController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost(nameof(UploadPostImage))]
        [ProducesResponseType(typeof(UploadPostImageResponseModel), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadPostImage([FromForm] UploadPostImageRequestModel model)
        {
            if (model.ImageFile == null || model.ImageFile.Length == 0)
                return BadRequest("Invalid file.");

            var command = new UploadPostImageCommand(model.ImageFile, model.FileId);
            var response = await _mediator.Send(command);
            return Accepted(response);
        }
    }
}

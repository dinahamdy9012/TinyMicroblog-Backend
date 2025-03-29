using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyMicroblog.Auth.API.Application.Models;
using TinyMicroblog.Auth.API.Application.UseCases.Auth;
using TinyMicroblog.Shared.Application.Models;

namespace TinyMicroblog.Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(DataResponse<LoginResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
        {
            var command = new UserLoginCommand(request);
            var response = await _mediator.Send(command);
            return Ok(response);
        }


        [HttpPost(nameof(RefreshToken))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel model)
        {
            if (string.IsNullOrEmpty(model.RefreshToken) || string.IsNullOrWhiteSpace(model.RefreshToken))
                return BadRequest();

            var command = new RefreshTokenCommand(model);
            var response = await _mediator.Send(command);
            return Accepted(response);
        }
    }
}

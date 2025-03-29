using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TinyMicroblog.Post.API.Application.Models;
using TinyMicroblog.Post.API.Application.UseCases.Post;
using TinyMicroblog.Shared.Application.Models;

namespace TinyMicroblog.Post.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetPosts")]
        [ProducesResponseType(typeof(DataResponse<List<PostModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPosts([FromQuery] FilterModel filterModel, [FromQuery] int screenSize)
        {
            if (filterModel.PageIndex <= 0 || filterModel.PageSize <= 0)
            { return BadRequest(); }

            PaginatedPostsQuery query = new PaginatedPostsQuery(filterModel, screenSize);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        [Route(nameof(CreatePost))]
        [ProducesResponseType(typeof(DataResponse<CreateEntityResponseModel>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequestModel model)
        {
            var command = new CreatePostCommand(model);
            var response = await _mediator.Send(command);
            return Accepted(response);
        }
    }
}

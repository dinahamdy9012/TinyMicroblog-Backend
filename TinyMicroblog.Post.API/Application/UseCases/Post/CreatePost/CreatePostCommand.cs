using MediatR;
using TinyMicroblog.Post.API.Application.Models;
using TinyMicroblog.Shared.Application.Models;

namespace TinyMicroblog.Post.API.Application.UseCases.Post
{
    public class CreatePostCommand : IRequest<DataResponse<CreateEntityResponseModel>>
    {
        public CreatePostRequestModel CreatePostRequestModel { get; }
        public CreatePostCommand(CreatePostRequestModel createPostRequestModel)
        {
            CreatePostRequestModel = createPostRequestModel;
        }
    }
}

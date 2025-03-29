using MediatR;
using TinyMicroblog.Post.API.Application.Interfaces;
using TinyMicroblog.Shared.Application.Models;

namespace TinyMicroblog.Post.API.Application.UseCases.Post
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, DataResponse<CreateEntityResponseModel>>
    {
        private readonly IPostService _postService;

        public CreatePostCommandHandler(IPostService postService)
        {
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }
        public async Task<DataResponse<CreateEntityResponseModel>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            return await _postService.CreatePostAsync(request.CreatePostRequestModel);
        }
    }
}

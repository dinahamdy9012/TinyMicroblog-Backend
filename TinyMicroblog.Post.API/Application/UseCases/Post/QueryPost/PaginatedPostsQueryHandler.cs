using MediatR;
using TinyMicroblog.Post.API.Application.Interfaces;
using TinyMicroblog.Post.API.Application.Models;
using TinyMicroblog.Shared.Application.Models;

namespace TinyMicroblog.Post.API.Application.UseCases.Post
{
    public class PaginatedPostsQueryHandler : IRequestHandler<PaginatedPostsQuery, DataResponse<List<PostModel>>>
    {
        private readonly IPostService _postService;

        public PaginatedPostsQueryHandler(IPostService postService)
        {
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        public async Task<DataResponse<List<PostModel>>> Handle(PaginatedPostsQuery request, CancellationToken cancellationToken)
        {
            return await _postService.GetPaginatedPosts(request.FilterModel, request.ScreenSize);
        }
    }
}

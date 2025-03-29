using TinyMicroblog.Post.API.Application.Models;
using TinyMicroblog.Shared.Application.Models;

namespace TinyMicroblog.Post.API.Application.Interfaces
{
    public interface IPostService
    {
        Task<DataResponse<List<PostModel>>> GetPaginatedPosts(FilterModel filterModel, int screenSize);
        Task<DataResponse<CreateEntityResponseModel>> CreatePostAsync(CreatePostRequestModel model);
    }
}

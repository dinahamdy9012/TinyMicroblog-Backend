using AutoMapper;
using MediatR;
using TinyMicroblog.Domain.Entities;
using TinyMicroblog.Domain.Interfaces.Post;
using TinyMicroblog.Post.API.Application.Interfaces;
using TinyMicroblog.Post.API.Application.Models;
using TinyMicroblog.Servicebus.Interfaces;
using TinyMicroblog.Shared.Application.Models;
using TinyMicroblog.Shared.Infrastructure.Security;
using TinyMicroblog.SharedKernel.Enums;
using TinyMicroblog.SharedKernel.Events;

namespace TinyMicroblog.Post.API.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IServiceBusService _serviceBusService;
        private readonly IConfiguration _configuration;

        public PostService(IPostRepository postRepository, IMapper mapper,
            ICurrentUserService currentUserService, IServiceBusService serviceBusService,
            IConfiguration configuration) 
        { 
            _postRepository = postRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _serviceBusService = serviceBusService;
            _configuration = configuration;
        }

        public async Task<DataResponse<List<PostModel>>> GetPaginatedPosts(FilterModel filterModel, int screenSize)
        {
            (var posts, var totalCount) = await _postRepository.GetPaginatedPostsAsync(filterModel.PageIndex, filterModel.PageSize, filterModel.SearchValue);
            return new DataResponse<List<PostModel>>().Success(_mapper.Map<List<PostModel>>(posts, x => x.Items["ScreenSize"]=screenSize), totalCount);
        }

        public async Task<DataResponse<CreateEntityResponseModel>> CreatePostAsync(CreatePostRequestModel model)
        {
            var user = _currentUserService.GetCurrentUser();
            var (latitude, longitude) = GenerateRandomCoordinates();
            var post = new Domain.Entities.Post
            {
                Latitude = (decimal)latitude,
                Longitude = (decimal)longitude,
                PostText = model.PostText,
                UserId = user.userId,
                Username = user.username
            };
            if (model.ImageUrl != null && !string.IsNullOrEmpty(model.ImageUrl.Trim())) {
                post.PostImages = new List<PostImage>() {
                    new PostImage()
                    {
                        ImageUrl = model.ImageUrl,
                        ImageType = nameof(ImageTypeEnum.Original)
                    }
                };

            }
           var entityId = await _postRepository.CreatePostAsync(post);
            await _serviceBusService.SendNotification(_configuration["AzureServicebusQueues:ConvertToWebpQueue"],
                new PostCreatedEvent(entityId, user.userId, model.ImageUrl));
            return new DataResponse<CreateEntityResponseModel>().Success(new CreateEntityResponseModel(entityId));
        }


        private (double Latitude, double Longitude) GenerateRandomCoordinates()
        {
            var random = new Random();

            double latitude = random.NextDouble() * 180 - 90;  // Range: -90 to 90
            double longitude = random.NextDouble() * 360 - 180; // Range: -180 to 180

            return (latitude, longitude);
        }
    }
}

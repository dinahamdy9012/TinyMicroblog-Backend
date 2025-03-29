using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TinyMicroblog.Domain.Interfaces.Post;
using TinyMicroblog.Post.API.Application.Models;
using TinyMicroblog.Post.API.Application.Services;
using TinyMicroblog.Servicebus.Interfaces;
using TinyMicroblog.Shared.Infrastructure.Security;

namespace TinyMicroblog.Post.API.UnitTest.CreatePost
{
    public class CreatePostTests
    {
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<ILogger<PostService>> _loggerMock;
        private readonly PostService _postService;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ICurrentUserService> _currentUserService;
        private readonly Mock<IServiceBusService> _servicebusService;
        private readonly Mock<IConfiguration> _configuration;

        public CreatePostTests()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _loggerMock = new Mock<ILogger<PostService>>();
            _mapper = new Mock<IMapper>();
            _currentUserService = new Mock<ICurrentUserService>();
            _configuration = new Mock<IConfiguration>();
            _servicebusService = new Mock<IServiceBusService>();
            _postService = new PostService(_postRepositoryMock.Object, _mapper.Object, _currentUserService.Object,
                _servicebusService.Object, _configuration.Object);
        }

        [Fact]
        public async Task CreatePost_Should_Return_PostId()
        {
            // Arrange
            var newPostRequestModel = new CreatePostRequestModel {PostText = "Hello World!", Latitude=90, Longitude=30 };
            var newPost = new Domain.Entities.Post { Id = 1, PostText = "Hello World!", Latitude = 90, Longitude = 30, UserId = 1, Username = "admin", CreatedAt=DateTime.UtcNow.AddDays(-1) };
            _postRepositoryMock.Setup(repo => repo.CreatePostAsync(It.IsAny<Domain.Entities.Post>())).ReturnsAsync(newPost.Id);

            // Act
            var result = await _postService.CreatePostAsync(newPostRequestModel);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.EntityId.Should().Be(newPost.Id);
            _postRepositoryMock.Verify(repo => repo.CreatePostAsync(It.IsAny<Domain.Entities.Post>()), Times.Once);
        }
    }
}

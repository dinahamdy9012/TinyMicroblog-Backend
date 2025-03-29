using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TinyMicroblog.Post.API.Application.Models;

namespace TinyMicroblog.Post.API.IntegrationTest.Controllers
{
    public class PostControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public PostControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            var fakeJwt = GenerateFakeJwtToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fakeJwt);
        }

        [Fact]
        public async Task CreatePost_Should_Return_202()
        {
            // Arrange
            var newPost = new CreatePostRequestModel { PostText = "New Post", Latitude = 90, Longitude = 90 };
            var content = new StringContent(JsonSerializer.Serialize(newPost), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/post/createpost", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Accepted);
        }

        [Fact]
        public async Task CreatePost_Should_Return_400()
        {
            // Arrange
            var newPost = new CreatePostRequestModel { PostText = "", Latitude = 90, Longitude = 90 };
            var content = new StringContent(JsonSerializer.Serialize(newPost), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/post/createpost", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task GetPaginatedPosts_Should_Return_200()
        {
            // Arrange
            var url = $"PageIndex=1&PageSize=10&screenSize=300";

            // Act
            var response = await _client.GetAsync("/api/post/GetPosts?"+url);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        private string GenerateFakeJwtToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyHere12345678ppplllxxxmmmvjdkdkkjdjufj!"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.UniqueName, "testuser"),
            new Claim(JwtRegisteredClaimNames.NameId, "1"),
        };

            var token = new JwtSecurityToken(
                issuer: "https://your-api.com",
                audience: "https://your-api.com",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

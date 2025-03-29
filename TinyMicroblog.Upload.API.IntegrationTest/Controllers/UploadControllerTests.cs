using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace TinyMicroblog.Upload.API.IntegrationTest.Controllers
{
    public class UploadControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public UploadControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            var fakeJwt = GenerateFakeJwtToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fakeJwt);
        }

        [Fact]
        public async Task UploadPostImage_Should_Return_202()
        {
            // Arrange
            var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("Fake Image Data"));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); // Set file type
            using var formData = new MultipartFormDataContent();
            formData.Add(fileContent, "imageFile", "test-image.jpg");

            // Act
            var response = await _client.PostAsync("/api/upload/UploadPostImage", formData);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Accepted);
        }

        [Fact]
        public async Task UploadPostImage_Should_Return_400()
        {
            // Arrange
            var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("Fake Image Data"));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/tiff"); // Set file type
            using var formData = new MultipartFormDataContent();
            formData.Add(fileContent, "imageFile", "test-image.tiff");

            // Act
            var response = await _client.PostAsync("/api/upload/UploadPostImage", formData);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UploadPostImage_BigSize_Should_Return_400()
        {
            // Arrange
            var fileContent = new ByteArrayContent(new byte[2097154]);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); // Set file type
            using var formData = new MultipartFormDataContent();
            formData.Add(fileContent, "imageFile", "test-image.jpg");

            // Act
            var response = await _client.PostAsync("/api/upload/UploadPostImage", formData);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
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

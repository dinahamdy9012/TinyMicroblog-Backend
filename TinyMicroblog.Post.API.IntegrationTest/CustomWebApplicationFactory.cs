using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TinyMicroblog.Shared.Infrastructure.Data;

namespace TinyMicroblog.Post.API.IntegrationTest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Test.json");
                config.AddJsonFile(path, optional: false, reloadOnChange: true);
            });
            builder.ConfigureServices(services =>
            {
                // Replace real DB with In-Memory DB
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TinyMicroblogDBContext>));
                if (descriptor != null) services.Remove(descriptor);

                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TinyMicroblogDBContext));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                var dbContextOptionsConfigDescriptor = services.SingleOrDefault(d =>
              d.ServiceType.FullName == "Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptionsConfiguration`1[[TinyMicroblog.Shared.Infrastructure.Data.TinyMicroblogDBContext, TinyMicroblog.Shared.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]");
                if (dbContextOptionsConfigDescriptor != null) services.Remove(dbContextOptionsConfigDescriptor);

                var contextOptionsDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions));
                if (contextOptionsDescriptor != null)
                {
                    services.Remove(contextOptionsDescriptor);
                }

                services.AddDbContext<TinyMicroblogDBContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));

                // 🔹 Remove existing authentication
                var authDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AuthenticationHandler<AuthenticationSchemeOptions>));
                if (authDescriptor != null) services.Remove(authDescriptor);

                var authDescriptors = services.Where(d => d.ServiceType == typeof(JwtBearerHandler)).ToList();
                foreach (var descriptorr in authDescriptors)
                {
                    services.Remove(descriptorr);
                }

                var authDescriptorss = services.Where(d => d.ServiceType == typeof(IAuthenticationSchemeProvider)).ToList();
                foreach (var descriptrrr in authDescriptors)
                {
                    services.Remove(descriptrrr);
                }

                var authServices = services
         .Where(s => s.ServiceType == typeof(IAuthenticationSchemeProvider) ||
                     s.ServiceType == typeof(JwtBearerHandler) ||
                     s.ServiceType.FullName!.Contains("Microsoft.AspNetCore.Authentication"))
         .ToList();
                foreach (var service in authServices)
                {
                    services.Remove(service);
                }

                var descriptorrr = services.SingleOrDefault(d => d.ServiceType == typeof(IConfigureOptions<AuthenticationOptions>));
                if (descriptorrr != null) services.Remove(descriptorrr);

                var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://your-api.com",
                    ValidAudience = "https://your-api.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyHere12345678ppplllxxxmmmvjdkdkkjdjufj!"))
                };
            });
                services.AddAuthorization();
                var sp = services.BuildServiceProvider();

                // Seed Test Data
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TinyMicroblogDBContext>();
                db.Database.EnsureCreated();
                SeedTestData(db);
            });
        }

        private void SeedTestData(TinyMicroblogDBContext db)
        {
            db.Posts.Add(new Domain.Entities.Post { Id = 1, PostText = "Test Post", UserId=1, Username="admin", Latitude=30, Longitude=90 });
            db.SaveChanges();

        }
    }
}

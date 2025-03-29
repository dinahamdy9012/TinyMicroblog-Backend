using Microsoft.AspNetCore.Mvc.Authorization;
using System.Reflection;
using TinyMicroblog.Auth.API;
using TinyMicroblog.Shared.Infrastructure;
using TinyMicroblog.Shared.Infrastructure.Settings;
using TinyMicroblog.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment.EnvironmentName;
Console.WriteLine(env);
builder.Configuration
    .AddJsonFile($"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\sharedsettings.{env}.json",
            optional: false,
            reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", true)
    .AddEnvironmentVariables();
builder.Services.AddControllers(options => options.Filters.Add(new AuthorizeFilter()));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.ConfigureSharedInfrastructureServices(builder.Configuration);
builder.Services.AddWebAPIServices();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

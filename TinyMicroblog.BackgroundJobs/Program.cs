using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using System.Reflection;
using TinyMicroblog.BackgroundJobs.BackgroundServices;
using TinyMicroblog.BackgroundJobs.EventHandlers;
using TinyMicroblog.BackgroundJobs.Interfaces;
using TinyMicroblog.BackgroundJobs.Services;
using TinyMicroblog.Domain.Interfaces.Post;
using TinyMicroblog.Servicebus.Interfaces;
using TinyMicroblog.Servicebus.Services;
using TinyMicroblog.Shared.Application.Interfaces;
using TinyMicroblog.Shared.Application.Services;
using TinyMicroblog.Shared.Infrastructure.Data;
using TinyMicroblog.Shared.Infrastructure.Repositories;
using TinyMicroblog.Shared.UploadService.Interfaces;
using TinyMicroblog.Shared.UploadService.Services;
using TinyMicroblog.SharedKernel.Events;

var builder = Host.CreateApplicationBuilder(args);
var env = builder.Environment.EnvironmentName;
Console.WriteLine(env);
builder.Configuration
    .AddJsonFile($"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\sharedsettings.{env}.json",
            optional: false,
            reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", true)
    .AddEnvironmentVariables();
builder.Services.AddDbContext<TinyMicroblogDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("TinyMicroblogDB")));
builder.Services.AddScoped<IPostImageService, PostImageService>();
builder.Services.AddScoped<IPostImageRepository, PostImageRepository>();

builder.Services.AddSingleton<IImageProcessingService, ImageProcessingService>();
builder.Services.AddScoped<IUploadService, AzureUploadService>();
builder.Services.AddScoped<IServiceBusService, ServiceBusService>();
builder.Services.AddTransient<INotificationHandler<ImageProcessedEvent>, ImageProcessedEventHandler>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"),
              new SqlServerStorageOptions
              {
                  CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                  SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                  QueuePollInterval = TimeSpan.Zero,
                  UseRecommendedIsolationLevel = true,
                  DisableGlobalLocks = true
              })
    );

// 🔹 Add Hangfire Server
builder.Services.AddHangfireServer();

builder.Services.AddHostedService<ConvertToWebpBackgroundService>();
builder.Services.AddHostedService<UpdatePostBackgroundService>();

var host = builder.Build();

host.Run();

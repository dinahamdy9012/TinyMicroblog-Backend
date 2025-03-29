using Azure.Messaging.ServiceBus;
using Hangfire;
using MediatR;
using System.Text.Json;
using TinyMicroblog.Domain.Interfaces.Post;
using TinyMicroblog.Shared.Application.Interfaces;
using TinyMicroblog.SharedKernel.Enums;
using TinyMicroblog.SharedKernel.Events;

namespace TinyMicroblog.BackgroundJobs.BackgroundServices
{
    public class UpdatePostBackgroundService : BackgroundService
    {
        private readonly ILogger<ConvertToWebpBackgroundService> _logger;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public UpdatePostBackgroundService(ILogger<ConvertToWebpBackgroundService> logger,
            IConfiguration configuration, IServiceScopeFactory serviceScopeFactory) 
        {
            _logger = logger;
            _client = new ServiceBusClient(configuration["ConnectionStrings:ServiceBus"]);
            _processor = _client.CreateProcessor(configuration["AzureServicebusQueues:UpdatePostQueue"], new ServiceBusProcessorOptions());
            _configuration = configuration;
            _scopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;

            await _processor.StartProcessingAsync(stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();

            var fileUploadedEvent = JsonSerializer.Deserialize<FileUploadedEvent>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignore case differences
            });

            if (fileUploadedEvent != null && fileUploadedEvent.EntityType == nameof(EntityTypeEnum.Post))
            {
                var scope = _scopeFactory.CreateScope();
                var postImageService = scope.ServiceProvider.GetRequiredService<IPostImageService>();
                await postImageService.UpdatePostImage(fileUploadedEvent.EntityId, fileUploadedEvent.FileUrl, fileUploadedEvent.FileType!);
            }

            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError($"Error processing message: {args.Exception}");
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            await _processor.DisposeAsync();
            await _client.DisposeAsync();
            _logger.LogInformation("Service Bus processor stopped.");
        }
    }
}

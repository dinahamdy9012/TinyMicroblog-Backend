using Azure.Messaging.ServiceBus;
using Hangfire;
using System.Text.Json;
using TinyMicroblog.BackgroundJobs.Interfaces;
using TinyMicroblog.SharedKernel.Enums;
using TinyMicroblog.SharedKernel.Events;

namespace TinyMicroblog.BackgroundJobs.BackgroundServices
{
    public class ConvertToWebpBackgroundService : BackgroundService
    {
        private readonly ILogger<ConvertToWebpBackgroundService> _logger;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private readonly IConfiguration _configuration;
        private readonly IImageProcessingService _imageProcessingService;
        public ConvertToWebpBackgroundService(ILogger<ConvertToWebpBackgroundService> logger,
            IConfiguration configuration, IImageProcessingService imageProcessingService)
        {
            _logger = logger;
            _client = new ServiceBusClient(configuration["ConnectionStrings:ServiceBus"]);
            _processor = _client.CreateProcessor(configuration["AzureServicebusQueues:ConvertToWebpQueue"], new ServiceBusProcessorOptions());
            _configuration = configuration;
            _imageProcessingService = imageProcessingService;
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

            var postCreatedEvent = JsonSerializer.Deserialize<PostCreatedEvent>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignore case differences
            });

            if (postCreatedEvent != null && postCreatedEvent.ImageUrl != null && !string.IsNullOrEmpty(postCreatedEvent.ImageUrl.Trim())) {
                   BackgroundJob.Enqueue(() => _imageProcessingService.ConvertToWebP(postCreatedEvent.PostId, nameof(EntityTypeEnum.Post), postCreatedEvent.ImageUrl, nameof(ImageTypeEnum.Original)));
                 BackgroundJob.Enqueue(() => _imageProcessingService.ResizeAndConvertToWebp(postCreatedEvent.PostId, nameof(EntityTypeEnum.Post), postCreatedEvent.ImageUrl));
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

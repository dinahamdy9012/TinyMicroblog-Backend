using Hangfire;
using MediatR;
using TinyMicroblog.Shared.UploadService.Interfaces;
using TinyMicroblog.Shared.UploadService.Models;
using TinyMicroblog.SharedKernel.Events;

namespace TinyMicroblog.BackgroundJobs.EventHandlers
{
    public class ImageProcessedEventHandler : INotificationHandler<ImageProcessedEvent>
    {
        private readonly IUploadService _uploadService;
        private readonly IConfiguration _configuration;
        public ImageProcessedEventHandler(IUploadService uploadService, IConfiguration configuration)
        {
            _uploadService = uploadService;
            _configuration = configuration;
        }

        public async Task Handle(ImageProcessedEvent notification, CancellationToken cancellationToken)
        {
            BackgroundJob.Enqueue(() => _uploadService.UploadFile(new UploadFileToAzureRequestModel
            {
                FileContent = notification.ImageStream,
                ContainerName = _configuration["AzureContainers:PostsPermenantContainer"],
                FileName = $"{Guid.NewGuid()}.webp",
                EntityId = notification.EntityId,
                EntityType = notification.EntityType,
                FileType = notification.ImageType
            }));
        }
    }
}

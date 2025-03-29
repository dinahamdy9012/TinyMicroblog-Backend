using MediatR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using TinyMicroblog.BackgroundJobs.Interfaces;
using TinyMicroblog.SharedKernel.Constants;
using TinyMicroblog.SharedKernel.Enums;
using TinyMicroblog.SharedKernel.Events;

namespace TinyMicroblog.BackgroundJobs.Services
{
    public class ImageProcessingService : IImageProcessingService
    {
        private readonly IMediator _mediator;
        public ImageProcessingService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task ConvertToWebP(int entityId, string entityType, string imageUrl, string? imageType)
        {
            using var httpClient = new HttpClient();
            using var imageStream = await httpClient.GetStreamAsync(imageUrl);
            using var image = await Image.LoadAsync(imageStream);

            var memoryStream = new MemoryStream();
            await image.SaveAsync(memoryStream, new WebpEncoder());
            memoryStream.Position = 0;

            await _mediator.Publish(new ImageProcessedEvent
            {
                EntityId = entityId,
                ImageStream = memoryStream.ToArray(),
                EntityType = entityType,
                ImageType = imageType
            });
        }

        public async Task ResizeAndConvertToWebp(int entityId, string entityType, string imageUrl)
        {
            using var httpClient = new HttpClient();
            using var imageStream = await httpClient.GetStreamAsync(imageUrl);
            using var image = await Image.LoadAsync(imageStream);

            foreach (var size in ImageResizeSizesConstant.IMAGE_RESIZE_SIZES)
            {
                var resizedImage = image.Clone(ctx => ctx.Resize(new ResizeOptions
                {
                    Size = new Size(size.Value, size.Value),
                    Mode = ResizeMode.Max
                }));

                var memoryStream = new MemoryStream();
                await resizedImage.SaveAsync(memoryStream, new WebpEncoder());
                memoryStream.Position = 0;

                await _mediator.Publish(new ImageProcessedEvent
                {
                    EntityId = entityId,
                    ImageStream = memoryStream.ToArray(),
                    EntityType = entityType,
                    ImageType = size.Key
                });
            }
        }
    }
}

using System;
namespace TinyMicroblog.BackgroundJobs.Interfaces
{
    public interface IImageProcessingService
    {
        Task ConvertToWebP(int entityId, string entityType, string imageUrl, string? imageType);
       Task ResizeAndConvertToWebp(int entityId, string entityType, string imageUrl);
    }
}

using MediatR;
namespace TinyMicroblog.SharedKernel.Events
{
    public class ImageProcessedEvent : EventBaseModel, INotification
    {
        public byte[] ImageStream { get; set; } = null!;
        public string? ImageType { get; set; }
    }
}

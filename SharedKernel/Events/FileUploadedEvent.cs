using MediatR;

namespace TinyMicroblog.SharedKernel.Events
{
    public class FileUploadedEvent : EventBaseModel, INotification
    {
        public string FileUrl { get; set; } = null!;

        public string? FileType { get; set; }
    }
}

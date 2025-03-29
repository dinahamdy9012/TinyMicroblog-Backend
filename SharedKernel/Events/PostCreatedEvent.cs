using MediatR;

namespace TinyMicroblog.SharedKernel.Events
{
    public class PostCreatedEvent : INotification
    {
        public int PostId { get; }
        public int UserId { get;}
        public string? ImageUrl { get; }
        public PostCreatedEvent(int postId, int userId, string? imageUrl)
        {
            PostId = postId;
            UserId = userId;
            ImageUrl = imageUrl;
        }
    }
}

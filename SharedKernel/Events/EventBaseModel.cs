namespace TinyMicroblog.SharedKernel.Events
{
    public class EventBaseModel
    {
        public int EntityId { get; set; }
        public string EntityType { get; set; } = null!;
    }
}

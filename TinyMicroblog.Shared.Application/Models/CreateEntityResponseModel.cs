namespace TinyMicroblog.Shared.Application.Models
{
    public class CreateEntityResponseModel
    {
        public int EntityId { get; }
        public CreateEntityResponseModel(int entityId) {  EntityId = entityId; }
    }
}

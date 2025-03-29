using System.ComponentModel.DataAnnotations;

namespace Upload.API.Application.Models
{
    public class UploadPostImageRequestModel
    {
        [Required]
        public IFormFile ImageFile { get; set; } = null!;
        public string? FileId { get; set; }

    }
}

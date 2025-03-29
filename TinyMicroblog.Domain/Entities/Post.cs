using System;
using System.Collections.Generic;

namespace TinyMicroblog.Domain.Entities;

public partial class Post
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PostText { get; set; } = null!;

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<PostImage> PostImages { get; set; } = new List<PostImage>();

    public virtual User User { get; set; } = null!;
}

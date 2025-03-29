using System;
using System.Collections.Generic;

namespace TinyMicroblog.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}

using System;

namespace Blog.Api.Domain.Models
{
    public class Bookmark
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using System;

namespace Blog.Api.Domain.Models
{
    public class Comment : BaseEntity
    {
        public Guid PostId { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? UserId { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorEmail { get; set; }
        public string Content { get; set; } = default!;
        public string Status { get; set; } = "pending"; // pending|approved|spam
    }
}

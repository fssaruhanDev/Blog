using System;

namespace Blog.Api.Domain.Models
{
    public class Reaction : BaseEntity
    {
        public string TargetType { get; set; } = default!; // post|comment
        public Guid TargetId { get; set; }
        public Guid? UserId { get; set; }
        public string Type { get; set; } = default!; // like|laugh|...
    }
}

using System;

namespace Blog.Api.Domain.Models
{
    public class Attachment : BaseEntity
    {
        public string TargetType { get; set; } = default!; // post|comment
        public Guid TargetId { get; set; }
        public Guid MediaId { get; set; }
        public string? Purpose { get; set; } // cover|gallery|inline
        public int? SortOrder { get; set; }
    }
}

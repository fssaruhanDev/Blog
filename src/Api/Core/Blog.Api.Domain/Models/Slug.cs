using System;

namespace Blog.Api.Domain.Models
{
    public class Slug : BaseEntity
    {
        public string TargetType { get; set; } = default!; // post|page|category|tag
        public Guid TargetId { get; set; }
        public string SlugText { get; set; } = default!;
        public bool IsPrimary { get; set; }
    }
}

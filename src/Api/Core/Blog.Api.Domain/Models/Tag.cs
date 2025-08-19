using System;

namespace Blog.Api.Domain.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public string ContentType { get; set; } = "project";
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
    }
}

using System;

namespace Blog.Api.Domain.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
    }
}

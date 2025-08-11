using System;

namespace Blog.Api.Domain.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
        public int? Order { get; set; }
    }
}

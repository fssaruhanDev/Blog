using System;
using System.Collections.Generic;

namespace Blog.Common.Models.Queries.Category
{
    public class CategoryListItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
        public string? ParentName { get; set; }
        public int Order { get; set; }
        public string ContentType { get; set; } = default!;
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public int PostCount { get; set; }
        public int ChildrenCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
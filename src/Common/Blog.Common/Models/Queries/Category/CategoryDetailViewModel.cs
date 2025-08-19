using System;
using System.Collections.Generic;

namespace Blog.Common.Models.Queries.Category
{
    public class CategoryDetailViewModel
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Related data
        public List<CategoryListItemViewModel> Children { get; set; } = new();
        public List<PostListItemViewModel> Posts { get; set; } = new();
        public int TotalPostCount { get; set; }
    }
    
    public class PostListItemViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Excerpt { get; set; }
        public string Status { get; set; } = default!;
        public DateTime? PublishedAt { get; set; }
        public string? CoverImageUrl { get; set; }
        public bool IsFeatured { get; set; }
    }
}
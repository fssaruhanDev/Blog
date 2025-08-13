using System;

namespace Blog.Api.Domain.Models
{
    public class Page : BaseEntity
    {
        public string Slug { get; set; } = string.Empty; // unique
        public string Title { get; set; } = string.Empty;
        public string? Excerpt { get; set; }
        public string? Content { get; set; } // HTML
        public string Status { get; set; } = "draft"; // draft|published
        public DateTime? PublishedAt { get; set; }
        public bool IsStandalone { get; set; } // nav menude göster
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}

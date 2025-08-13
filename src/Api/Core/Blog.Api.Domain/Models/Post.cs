using System;

namespace Blog.Api.Domain.Models
{
    public class Post : BaseEntity
    {
        public Guid AuthorId { get; set; }
        public required string Title { get; set; }
        public string? Excerpt { get; set; }
        public string? Content { get; set; }
        public string Status { get; set; } = "draft"; // draft|published|scheduled
        public DateTime? PublishedAt { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public int? ReadingTime { get; set; }
    public bool IsFeatured { get; set; }
    // CoverMediaId kaldırıldı (kullanılmıyor)
    public string? CoverImageUrl { get; set; } // simple external URL or uploaded media reference
    }
}

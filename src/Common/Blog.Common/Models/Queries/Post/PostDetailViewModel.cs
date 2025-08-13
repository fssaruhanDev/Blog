using System;

namespace Blog.Common.Models.Queries.Post
{
    public class PostDetailViewModel
    {
        public Guid ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Excerpt { get; set; }
        public string? Content { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? PublishedAt { get; set; }
    public string? CoverImageUrl { get; set; }
    }
}

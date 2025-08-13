using System;

namespace Blog.Common.Models.Queries.Pages
{
    public class PageListItemViewModel
    {
        public Guid ID { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Excerpt { get; set; }
        public string Status { get; set; } = "draft";
        public DateTime CreatedDate { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}

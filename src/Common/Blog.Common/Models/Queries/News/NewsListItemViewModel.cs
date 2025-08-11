using System;

namespace Blog.Common.Models.Queries.News
{
    public class NewsListItemViewModel
    {
        public Guid ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string Status { get; set; } = "draft";
        public DateTime CreatedDate { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? SourceName { get; set; }
        public string? SourceUrl { get; set; }
        public string? Tags { get; set; }
    }
}

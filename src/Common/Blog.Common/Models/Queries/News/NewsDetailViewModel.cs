namespace Blog.Common.Models.Queries.News
{
    public class NewsDetailViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Summary { get; set; }
        public string? SourceName { get; set; }
        public string? SourceUrl { get; set; }
        public string Status { get; set; } = "draft";
        public DateTime? PublishedAt { get; set; }
        public string? Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
namespace Blog.Common.Models.RequestModels.News
{
    public class UpdateNewsRequest
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Summary { get; set; }
        public string? SourceName { get; set; }
        public string? SourceUrl { get; set; }
        public string Status { get; set; } = "draft";
        public DateTime? PublishedAt { get; set; }
        public string? Tags { get; set; }
    }
}
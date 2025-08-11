using System;

namespace Blog.Api.Domain.Models
{
    public class News : BaseEntity
    {
        public required string Title { get; set; }
        public string? Summary { get; set; }
        public string? SourceName { get; set; }
        public string? SourceUrl { get; set; }
        public string Status { get; set; } = "draft"; // draft|published|archived
        public DateTime? PublishedAt { get; set; }
        // Basit etiketleme: virgülle ayrılmış liste
        public string? Tags { get; set; }
    }
}

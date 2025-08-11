using System;

namespace Blog.Api.Domain.Models
{
    public class Media : BaseEntity
    {
        public string FileName { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string MimeType { get; set; } = default!;
        public long Size { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? DurationSec { get; set; }
        public string? AltText { get; set; }
    }
}

using System;

namespace Blog.Api.Domain.Models
{
    public class SeoMeta : BaseEntity
    {
        public string TargetType { get; set; } = default!; // post|page|category|tag
        public Guid TargetId { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? OgTitle { get; set; }
        public string? OgDescription { get; set; }
        public Guid? OgImageId { get; set; }
        public string? CanonicalUrl { get; set; }
        public string? Robots { get; set; }
        public string? JsonLd { get; set; }
    }
}

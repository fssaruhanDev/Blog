using Blog.Api.Domain.Models;

namespace Blog.Api.Domain.Models
{
    public class Project : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? Content { get; set; } // Detaylı açıklama (HTML)
        public string? FeaturedImage { get; set; }
        public List<string>? Images { get; set; } // JSON array
        public string? TechnologiesUsed { get; set; } // JSON array
        public string? ProjectUrl { get; set; } // Canlı URL
        public string? GithubUrl { get; set; }
        public string? DemoUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public ProjectType Type { get; set; }
        public string? ClientName { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsPublic { get; set; } = true;
        public int ViewCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public int Order { get; set; } = 0;
        
        // Categories relation (many-to-many)
        public List<ProjectCategory>? ProjectCategories { get; set; }
        
        // Tags relation (many-to-many)
        public List<ProjectTag>? ProjectTags { get; set; }
    }

    public enum ProjectStatus
    {
        Planning = 1,
        InProgress = 2,
        Completed = 3,
        OnHold = 4,
        Cancelled = 5
    }

    public enum ProjectType
    {
        WebApplication = 1,
        MobileApplication = 2,
        DesktopApplication = 3,
        Website = 4,
        ECommerce = 5,
        API = 6,
        Library = 7,
        Plugin = 8,
        Theme = 9,
        Other = 10
    }
}
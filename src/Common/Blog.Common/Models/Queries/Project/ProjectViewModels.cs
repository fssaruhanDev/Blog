namespace Blog.Common.Models.Queries.Project
{
    public class ProjectListItemViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? FeaturedImage { get; set; }
        public List<string>? TechnologiesUsed { get; set; }
        public string? ProjectUrl { get; set; }
        public string? GithubUrl { get; set; }
        public ProjectStatus Status { get; set; }
        public ProjectType Type { get; set; }
        public string? ClientName { get; set; }
        public bool IsFeatured { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Category info
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategorySlug { get; set; }
        
        // Tags
        public List<ProjectTagViewModel>? Tags { get; set; }
    }

    public class ProjectDetailViewModel : ProjectListItemViewModel
    {
        public string? Content { get; set; }
        public List<string>? Images { get; set; }
        public string? DemoUrl { get; set; }
        public List<ProjectListItemViewModel>? RelatedProjects { get; set; }
    }

    public class ProjectTagViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Color { get; set; }
        public int ProjectCount { get; set; }
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
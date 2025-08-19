using Blog.Common.Models.Queries.Project;

namespace Blog.Common.Models.RequestModels.Project
{
    public class CreateProjectRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        public string? FeaturedImage { get; set; }
        public List<string>? Images { get; set; }
        public List<string>? TechnologiesUsed { get; set; }
        public string? ProjectUrl { get; set; }
        public string? GithubUrl { get; set; }
        public string? DemoUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public ProjectType Type { get; set; }
        public string? ClientName { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsPublic { get; set; } = true;
        public int Order { get; set; } = 0;
        public Guid? CategoryId { get; set; }
        public List<Guid>? TagIds { get; set; }
    }

    public class UpdateProjectRequest : CreateProjectRequest
    {
        public Guid Id { get; set; }
    }

    public class CreateProjectTagRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateProjectTagRequest : CreateProjectTagRequest
    {
        public Guid Id { get; set; }
    }
}
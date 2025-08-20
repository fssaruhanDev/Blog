using MediatR;
using Blog.Common.Models.Event.Project;

namespace Blog.Api.Application.Commands.Project.UpdateProject;

public class UpdateProjectCommand : IRequest<UpdateProjectModel>
{
    public Guid Id { get; set; }
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
    public int Status { get; set; }
    public int Type { get; set; }
    public string? ClientName { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublic { get; set; } = true;
    public int Order { get; set; } = 0;
    public Guid? CategoryId { get; set; }
    public List<Guid>? TagIds { get; set; }
    public List<string>? NewTags { get; set; }
}

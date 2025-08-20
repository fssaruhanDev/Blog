using MediatR;
using Blog.Common.Models.Queries.Project;
using Blog.Common.Models.Queries;

namespace Blog.Api.Application.Queries.Project.GetPublicProjects;

public class GetPublicProjectsQuery : IRequest<PagedResult<ProjectListItemViewModel>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Search { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? TagId { get; set; }
    public ProjectType? Type { get; set; }
    public ProjectStatus? Status { get; set; }
    public bool Featured { get; set; } = false;
}

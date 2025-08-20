using MediatR;
using Blog.Common.Models.Queries;
using Blog.Common.Models.Queries.Project;

namespace Blog.Api.Application.Queries.Project.GetProjects;

public class GetProjectsQuery : IRequest<PagedResult<ProjectListItemViewModel>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Search { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? TagId { get; set; }
}

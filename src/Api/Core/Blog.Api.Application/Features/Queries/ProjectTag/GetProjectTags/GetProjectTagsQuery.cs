using MediatR;
using Blog.Common.Models.Queries.Project;

namespace Blog.Api.Application.Queries.ProjectTag.GetProjectTags;

public class GetProjectTagsQuery : IRequest<List<ProjectTagViewModel>>
{
    public string? Search { get; set; }
    public int Limit { get; set; } = 100;
}

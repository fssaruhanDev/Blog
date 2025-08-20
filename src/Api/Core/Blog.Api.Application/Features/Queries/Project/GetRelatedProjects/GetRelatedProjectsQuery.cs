using MediatR;
using Blog.Common.Models.Queries.Project;

namespace Blog.Api.Application.Queries.Project.GetRelatedProjects;

public class GetRelatedProjectsQuery : IRequest<List<ProjectListItemViewModel>>
{
    public Guid ProjectId { get; set; }
    public int Count { get; set; } = 4;
}

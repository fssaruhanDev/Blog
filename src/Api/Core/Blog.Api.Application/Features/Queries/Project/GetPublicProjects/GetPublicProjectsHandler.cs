using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries;
using Blog.Common.Models.Queries.Project;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Application.Queries.Project.GetPublicProjects;

public class GetPublicProjectsHandler : IRequestHandler<GetPublicProjectsQuery, PagedResult<ProjectListItemViewModel>>
{
    private readonly IProjectRepository _projectRepository;

    public GetPublicProjectsHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<PagedResult<ProjectListItemViewModel>> Handle(GetPublicProjectsQuery request, CancellationToken cancellationToken)
    {
        // Delegate to previously implemented GetProjectsQueryHandler logic by creating a GetProjectsQuery
        var q = new Blog.Api.Application.Queries.Project.GetProjects.GetProjectsQuery
        {
            Page = request.Page,
            PageSize = request.PageSize,
            Search = request.Search,
            CategoryId = request.CategoryId,
            TagId = null
        };

        var handler = new Blog.Api.Application.Queries.Project.GetProjects.GetProjectsQueryHandler(_projectRepository);
        return await handler.Handle(q, cancellationToken);
    }
}

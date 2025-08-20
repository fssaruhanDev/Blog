using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries.Project;
using MediatR;

namespace Blog.Api.Application.Queries.Project.GetRelatedProjects;

public class GetRelatedProjectsHandler : IRequestHandler<GetRelatedProjectsQuery, List<ProjectListItemViewModel>>
{
    private readonly IProjectRepository _projectRepository;

    public GetRelatedProjectsHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<List<ProjectListItemViewModel>> Handle(GetRelatedProjectsQuery request, CancellationToken cancellationToken)
    {
        var related = await _projectRepository.GetRelatedProjectsAsync(request.ProjectId, request.Count);
        // simple mapping
        return related.Select(p => new ProjectListItemViewModel
        {
            Id = p.ID,
            Title = p.Title,
            Description = p.Description,
            ShortDescription = p.ShortDescription,
            Slug = p.Slug,
            FeaturedImage = p.FeaturedImage,
            TechnologiesUsed = string.IsNullOrEmpty(p.TechnologiesUsed) ? null : p.TechnologiesUsed.Split(',').ToList(),
            ProjectUrl = p.ProjectUrl,
            GithubUrl = p.GithubUrl,
            Status = (Blog.Common.Models.Queries.Project.ProjectStatus)p.Status,
            Type = (Blog.Common.Models.Queries.Project.ProjectType)p.Type,
            ClientName = p.ClientName,
            IsFeatured = p.IsFeatured,
            ViewCount = p.ViewCount,
            LikeCount = p.LikeCount,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            CreatedAt = p.CreatedDate,
            UpdatedAt = p.UpdatedDate ?? p.CreatedDate
        }).ToList();
    }
}

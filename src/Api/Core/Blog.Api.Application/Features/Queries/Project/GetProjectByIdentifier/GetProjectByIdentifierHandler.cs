using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries.Project;
using MediatR;

namespace Blog.Api.Application.Queries.Project.GetProjectByIdentifier;

public class GetProjectByIdentifierHandler : IRequestHandler<GetProjectByIdentifierQuery, ProjectDetailViewModel?>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectByIdentifierHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDetailViewModel?> Handle(GetProjectByIdentifierQuery request, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(request.Identifier, out var id))
        {
            var p = await _projectRepository.GetProjectWithDetailsAsync(id);
            if (p == null) return null;
            // map to ProjectDetailViewModel similar to existing GetProjectByIdHandler
            return await MapProject(p);
        }

        var bySlug = await _projectRepository.GetProjectBySlugAsync(request.Identifier);
        if (bySlug == null) return null;
        return await MapProject(bySlug);
    }

    private Task<ProjectDetailViewModel> MapProject(Blog.Api.Domain.Models.Project p)
    {
        var vm = new ProjectDetailViewModel
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
            DemoUrl = p.DemoUrl,
            Status = (Blog.Common.Models.Queries.Project.ProjectStatus)p.Status,
            Type = (Blog.Common.Models.Queries.Project.ProjectType)p.Type,
            ClientName = p.ClientName,
            IsFeatured = p.IsFeatured,
            ViewCount = p.ViewCount,
            LikeCount = p.LikeCount,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            CreatedAt = p.CreatedDate,
            UpdatedAt = p.UpdatedDate ?? p.CreatedDate,
            Content = p.Content,
            Images = p.Images,
            RelatedProjects = new List<ProjectListItemViewModel>()
        };
        return Task.FromResult(vm);
    }
}

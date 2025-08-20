using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries.Project;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Application.Queries.Project.GetProjectById;

public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectDetailViewModel?>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectByIdHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDetailViewModel?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
    var p = await _projectRepository.GetProjectWithDetailsAsync(request.Id);
        if (p == null) return null;

        return new ProjectDetailViewModel
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
    }
}

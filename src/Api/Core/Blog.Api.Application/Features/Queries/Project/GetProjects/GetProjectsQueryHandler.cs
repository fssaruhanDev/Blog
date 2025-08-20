using System.Linq;
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries;
using Blog.Common.Models.Queries.Project;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Application.Queries.Project.GetProjects;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, PagedResult<ProjectListItemViewModel>>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectsQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<PagedResult<ProjectListItemViewModel>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 || request.PageSize > 100 ? 20 : request.PageSize;

        var query = _projectRepository.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(p => p.Title.Contains(request.Search));

        if (request.CategoryId.HasValue)
            query = query.Where(p => p.ProjectCategories != null && p.ProjectCategories.Any(pc => pc.ProjectId == request.CategoryId.Value));

        if (request.TagId.HasValue)
            query = query.Where(p => p.ProjectTags != null && p.ProjectTags.Any(t => t.ID == request.TagId.Value));

        var total = await query.CountAsync(cancellationToken);

        var raw = await query
            .OrderByDescending(p => p.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new
            {
                p.ID,
                p.Title,
                p.Description,
                p.ShortDescription,
                p.Slug,
                p.FeaturedImage,
                p.TechnologiesUsed,
                p.ProjectUrl,
                p.GithubUrl,
                p.Status,
                p.Type,
                p.ClientName,
                p.IsFeatured,
                p.ViewCount,
                p.LikeCount,
                p.StartDate,
                p.EndDate,
                p.CreatedDate,
                p.UpdatedDate,
                Categories = p.ProjectCategories != null ? p.ProjectCategories.Select(pc => pc.CategoryId) : new Guid[] { }
            })
            .ToArrayAsync(cancellationToken);

        var items = raw.Select(p => new ProjectListItemViewModel
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
            UpdatedAt = p.UpdatedDate ?? p.CreatedDate,
            CategoryId = p.Categories != null && p.Categories.Any() ? p.Categories.First() : (Guid?)null
        }).ToArray();

        return new PagedResult<ProjectListItemViewModel>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = items
        };
    }
}

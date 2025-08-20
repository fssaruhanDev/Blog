using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries.Project;
using MediatR;

namespace Blog.Api.Application.Queries.ProjectTag.GetProjectTags;

public class GetProjectTagsHandler : IRequestHandler<GetProjectTagsQuery, List<ProjectTagViewModel>>
{
    private readonly IProjectTagRepository _tagRepository;

    public GetProjectTagsHandler(IProjectTagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<List<ProjectTagViewModel>> Handle(GetProjectTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.GetAllTagsAsync(request.Limit);

        if (!string.IsNullOrWhiteSpace(request.Search))
            tags = tags.Where(t => t.Name != null && t.Name.Contains(request.Search, StringComparison.OrdinalIgnoreCase)).ToList();

        return tags.Select(t => new ProjectTagViewModel
        {
            Id = t.ID,
            Name = t.Name,
            Slug = t.Slug,
            Color = t.Color,
            ProjectCount = t.Projects?.Count ?? 0
        }).ToList();
    }
}

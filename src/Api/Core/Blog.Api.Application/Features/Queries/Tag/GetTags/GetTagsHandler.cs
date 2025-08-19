using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries.Tag;
using MediatR;

namespace Blog.Api.Application.Queries.Tag.GetTags;

public class GetTagsHandler : IRequestHandler<GetTagsQuery, List<TagViewModel>>
{
    private readonly ITagRepository _tagRepository;

    public GetTagsHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<List<TagViewModel>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.GetAll();

        // Apply filters
        if (!string.IsNullOrEmpty(request.ContentType))
        {
            tags = tags.Where(t => t.ContentType == request.ContentType).ToList();
        }

        if (request.IsActive.HasValue)
        {
            tags = tags.Where(t => t.IsActive == request.IsActive.Value).ToList();
        }

        if (request.IsFeatured.HasValue)
        {
            tags = tags.Where(t => t.IsFeatured == request.IsFeatured.Value).ToList();
        }

        if (!string.IsNullOrEmpty(request.Search))
        {
            tags = tags.Where(t => 
                t.Name.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                (t.Description != null && t.Description.Contains(request.Search, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        // Apply limit if specified
        if (request.Limit.HasValue && request.Limit.Value > 0)
        {
            tags = tags.Take(request.Limit.Value).ToList();
        }

        return tags
            .OrderBy(t => t.Name)
            .Select(t => new TagViewModel
            {
                Id = t.ID,
                Name = t.Name,
                Description = t.Description,
                ContentType = t.ContentType,
                Color = t.Color,
                Icon = t.Icon,
                IsActive = t.IsActive,
                IsFeatured = t.IsFeatured,
                ProjectCount = 0, // TODO: Implement actual count
                CreatedDate = t.CreatedDate
            }).ToList();
    }
}
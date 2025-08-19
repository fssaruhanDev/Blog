using Blog.Api.Domain.Interfaces.Repositories;
using MediatR;

namespace Blog.Api.Application.Queries.Tag.GetPublicTags;

public class GetPublicTagsHandler : IRequestHandler<GetPublicTagsQuery, List<GetPublicTagsResponse>>
{
    private readonly ITagRepository _tagRepository;

    public GetPublicTagsHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<List<GetPublicTagsResponse>> Handle(GetPublicTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.GetAll();

        // Only return active tags for public access
        tags = tags.Where(t => t.IsActive).ToList();

        // Apply content type filter if provided
        if (!string.IsNullOrEmpty(request.ContentType))
        {
            tags = tags.Where(t => t.ContentType == request.ContentType).ToList();
        }

        return tags
            .OrderBy(t => t.Name)
            .Select(t => new GetPublicTagsResponse
            {
                Id = t.ID,
                Name = t.Name,
                Description = t.Description,
                ContentType = t.ContentType,
                Color = t.Color,
                Icon = t.Icon,
                ProjectCount = 0 // TODO: Implement actual count
            }).ToList();
    }
}
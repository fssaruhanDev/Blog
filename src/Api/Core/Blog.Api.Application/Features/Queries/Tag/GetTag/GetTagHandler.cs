using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries.Tag;
using MediatR;
using Blog.Common.Infrastructure.Exeptions;

namespace Blog.Api.Application.Queries.Tag.GetTag;

public class GetTagHandler : IRequestHandler<GetTagQuery, TagViewModel>
{
    private readonly ITagRepository _tagRepository;

    public GetTagHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<TagViewModel> Handle(GetTagQuery request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(request.Id);
        
        if (tag == null)
        {
            throw new NotFoundException("Tag not found");
        }

        return new TagViewModel
        {
            Id = tag.ID,
            Name = tag.Name,
            Description = tag.Description,
            ContentType = tag.ContentType,
            Color = tag.Color,
            Icon = tag.Icon,
            IsActive = tag.IsActive,
            IsFeatured = tag.IsFeatured,
            ProjectCount = 0, // TODO: Implement actual count
            CreatedDate = tag.CreatedDate,
            UpdatedDate = tag.UpdatedDate
        };
    }
}
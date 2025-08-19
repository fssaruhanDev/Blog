using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Event.Tag;
using MediatR;

namespace Blog.Api.Application.Commands.Tag.UpdateTag;

public class UpdateTagHandler : IRequestHandler<UpdateTagCommand, UpdateTagModel>
{
    private readonly ITagRepository _tagRepository;

    public UpdateTagHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<UpdateTagModel> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(request.Id);
        
        if (tag == null)
        {
            throw new InvalidOperationException("Tag not found");
        }

        tag.Name = request.Name;
        tag.Description = request.Description;
        tag.ContentType = request.ContentType;
        tag.Color = request.Color;
        tag.Icon = request.Icon;
        tag.IsActive = request.IsActive;
        tag.IsFeatured = request.IsFeatured;
        tag.UpdatedDate = DateTime.UtcNow;

        await _tagRepository.UpdateAsync(tag);

        return new UpdateTagModel
        {
            Id = tag.ID,
            Name = tag.Name,
            Description = tag.Description,
            UpdatedDate = tag.UpdatedDate ?? DateTime.UtcNow
        };
    }
}
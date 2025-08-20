using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Event.Tag;
using MediatR;

namespace Blog.Api.Application.Commands.Tag.CreateTag;

public class CreateTagHandler : IRequestHandler<CreateTagCommand, CreateTagModel>
{
    private readonly ITagRepository _tagRepository;

    public CreateTagHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<CreateTagModel> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = new Blog.Api.Domain.Models.Tag
        {
            ID = Blog.Common.Helpers.UlidHelper.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            ContentType = request.ContentType,
            Color = request.Color,
            Icon = request.Icon,
            IsActive = request.IsActive,
            IsFeatured = request.IsFeatured,
            CreatedDate = DateTime.UtcNow
        };

        await _tagRepository.AddAsync(tag);

        return new CreateTagModel
        {
            Id = tag.ID,
            Name = tag.Name,
            Description = tag.Description,
            CreatedDate = tag.CreatedDate
        };
    }
}
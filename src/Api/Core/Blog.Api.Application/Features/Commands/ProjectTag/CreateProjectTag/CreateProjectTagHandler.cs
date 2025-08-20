using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Event.Tag;
using MediatR;

namespace Blog.Api.Application.Commands.ProjectTag.CreateProjectTag;

public class CreateProjectTagHandler : IRequestHandler<CreateProjectTagCommand, CreateTagModel>
{
    private readonly IProjectTagRepository _tagRepository;

    public CreateProjectTagHandler(IProjectTagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<CreateTagModel> Handle(CreateProjectTagCommand request, CancellationToken cancellationToken)
    {
        var tag = new Blog.Api.Domain.Models.ProjectTag
        {
            ID = Blog.Common.Helpers.UlidHelper.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Color = request.Color,
            IsActive = request.IsActive,
            CreatedDate = DateTime.UtcNow,
            Slug = Blog.Common.Helpers.SlugHelper.GenerateSlug(request.Name)
        };

        await _tagRepository.AddAsync(tag);

        return new CreateTagModel { Id = tag.ID, Name = tag.Name, CreatedDate = tag.CreatedDate };
    }
}

using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Event.Tag;
using MediatR;

namespace Blog.Api.Application.Commands.ProjectTag.UpdateProjectTag;

public class UpdateProjectTagHandler : IRequestHandler<UpdateProjectTagCommand, UpdateTagModel>
{
    private readonly IProjectTagRepository _tagRepository;

    public UpdateProjectTagHandler(IProjectTagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<UpdateTagModel> Handle(UpdateProjectTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(request.Id);
        if (tag == null) throw new Blog.Common.Infrastructure.Exeptions.NotFoundException("Tag not found");

        tag.Name = request.Name;
        tag.Description = request.Description;
        tag.Color = request.Color;
        tag.IsActive = request.IsActive;
        tag.UpdatedDate = DateTime.UtcNow;

        await _tagRepository.UpdateAsync(tag);

    return new UpdateTagModel { Id = tag.ID, Name = tag.Name, UpdatedDate = tag.UpdatedDate ?? tag.CreatedDate };
    }
}

using Blog.Api.Domain.Interfaces.Repositories;
using MediatR;

namespace Blog.Api.Application.Commands.ProjectTag.DeleteProjectTag;

public class DeleteProjectTagHandler : IRequestHandler<DeleteProjectTagCommand, bool>
{
    private readonly IProjectTagRepository _tagRepository;

    public DeleteProjectTagHandler(IProjectTagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<bool> Handle(DeleteProjectTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(request.Id);
        if (tag == null) return false;

        await _tagRepository.DeleteAsync(tag);
        return true;
    }
}

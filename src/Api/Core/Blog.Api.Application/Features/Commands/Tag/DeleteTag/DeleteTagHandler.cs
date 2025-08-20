using Blog.Api.Domain.Interfaces.Repositories;
using MediatR;
using Blog.Common.Infrastructure.Exeptions;

namespace Blog.Api.Application.Commands.Tag.DeleteTag;

public class DeleteTagHandler : IRequestHandler<DeleteTagCommand>
{
    private readonly ITagRepository _tagRepository;

    public DeleteTagHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(request.Id);
        
        if (tag == null)
        {
            throw new NotFoundException("Tag not found");
        }

        // Check if tag has related projects (unless force delete)
        if (!request.ForceDelete)
        {
            // You might want to implement checks here for related data
            // For now, just proceed with deletion
        }

        await _tagRepository.DeleteAsync(tag);
        return Unit.Value;
    }
}
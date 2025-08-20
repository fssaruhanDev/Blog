using MediatR;
using Blog.Api.Domain.Interfaces.Repositories;

namespace Blog.Api.Application.Commands.Project.IncrementViewCount;

public class IncrementViewCountHandler : IRequestHandler<IncrementViewCountCommand>
{
    private readonly IProjectRepository _projectRepository;

    public IncrementViewCountHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Unit> Handle(IncrementViewCountCommand request, CancellationToken cancellationToken)
    {
        await _projectRepository.IncrementViewCountAsync(request.Id);
        return Unit.Value;
    }
}

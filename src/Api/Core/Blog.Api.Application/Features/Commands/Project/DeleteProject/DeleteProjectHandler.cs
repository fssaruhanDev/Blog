using Blog.Api.Domain.Interfaces.Repositories;
using MediatR;

namespace Blog.Api.Application.Commands.Project.DeleteProject;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IProjectRepository _projectRepository;

    public DeleteProjectHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id);
        if (project == null) return false;

        await _projectRepository.DeleteAsync(project);
        return true;
    }
}

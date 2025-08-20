using MediatR;

namespace Blog.Api.Application.Commands.Project.DeleteProject;

public class DeleteProjectCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

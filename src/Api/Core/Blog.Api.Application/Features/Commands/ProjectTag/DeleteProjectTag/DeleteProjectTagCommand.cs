using MediatR;

namespace Blog.Api.Application.Commands.ProjectTag.DeleteProjectTag;

public class DeleteProjectTagCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

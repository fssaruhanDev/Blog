using MediatR;

namespace Blog.Api.Application.Commands.Tag.DeleteTag;

public class DeleteTagCommand : IRequest
{
    public Guid Id { get; set; }
    public bool ForceDelete { get; set; } = false;
}
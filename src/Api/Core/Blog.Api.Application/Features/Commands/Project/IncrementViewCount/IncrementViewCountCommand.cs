using MediatR;

namespace Blog.Api.Application.Commands.Project.IncrementViewCount;

public class IncrementViewCountCommand : IRequest
{
    public Guid Id { get; set; }
}

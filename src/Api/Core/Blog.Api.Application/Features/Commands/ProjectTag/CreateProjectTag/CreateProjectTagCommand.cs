using MediatR;
using Blog.Common.Models.Event.Tag;

namespace Blog.Api.Application.Commands.ProjectTag.CreateProjectTag;

public class CreateProjectTagCommand : IRequest<CreateTagModel>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Color { get; set; }
    public bool IsActive { get; set; } = true;
}

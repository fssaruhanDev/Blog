using MediatR;
using Blog.Common.Models.Event.Tag;

namespace Blog.Api.Application.Commands.Tag.UpdateTag;

public class UpdateTagCommand : IRequest<UpdateTagModel>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ContentType { get; set; } = "project";
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; } = false;
}
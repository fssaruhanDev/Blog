using MediatR;

namespace Blog.Api.Application.Commands.Comment.CreateComment;

public class CreateCommentCommand : IRequest<object>
{
    public Guid PostId { get; set; }
    public string? AuthorName { get; set; }
    public string? AuthorEmail { get; set; }
    public string Content { get; set; } = string.Empty;
}

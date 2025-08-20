using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;
using MediatR;

namespace Blog.Api.Application.Commands.Comment.CreateComment;

public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, object>
{
    private readonly IGenericRepository<Blog.Api.Domain.Models.Comment> _repo;

    public CreateCommentHandler(IGenericRepository<Blog.Api.Domain.Models.Comment> repo)
    {
        _repo = repo;
    }

    public async Task<object> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new Blog.Api.Domain.Models.Comment
        {
            ID = Blog.Common.Helpers.UlidHelper.NewGuid(),
            PostId = request.PostId,
            Content = (request.Content ?? string.Empty).Trim(),
            AuthorName = (request.AuthorName ?? "Anonim").Trim(),
            AuthorEmail = request.AuthorEmail,
            Status = "approved",
        };

        await _repo.AddAsync(comment);
        return new { comment.ID, comment.Content, comment.AuthorName, comment.CreatedDate };
    }
}

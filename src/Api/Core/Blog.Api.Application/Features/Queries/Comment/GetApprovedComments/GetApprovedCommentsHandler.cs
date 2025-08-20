using Blog.Api.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Application.Queries.Comment.GetApprovedComments;

public class GetApprovedCommentsHandler : IRequestHandler<GetApprovedCommentsQuery, List<object>>
{
    private readonly IGenericRepository<Blog.Api.Domain.Models.Comment> _repo;

    public GetApprovedCommentsHandler(IGenericRepository<Blog.Api.Domain.Models.Comment> repo)
    {
        _repo = repo;
    }

    public async Task<List<object>> Handle(GetApprovedCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _repo.Get(c => c.PostId == request.PostId && c.Status == "approved")
            .OrderBy(c => c.CreatedDate)
            .Select(c => new { c.ID, c.Content, c.AuthorName, c.CreatedDate })
            .ToListAsync(cancellationToken);

        return comments.Cast<object>().ToList();
    }
}

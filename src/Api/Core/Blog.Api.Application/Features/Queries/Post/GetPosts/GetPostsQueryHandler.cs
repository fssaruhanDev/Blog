using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Api.Application.Features.Queries.Post.GetPosts;
using Blog.Api.Application.Interfaces.Repostrories;
using DomainPost = Blog.Api.Domain.Models.Post;
using Blog.Common.Models.Queries;
using Blog.Common.Models.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Blog.Api.Application.Features.Queries.Post.GetPosts
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, PagedResult<PostListItemViewModel>>
    {
    private readonly IGenericRepository<DomainPost> _repo;
    private readonly IHttpContextAccessor _http;
    public GetPostsQueryHandler(IGenericRepository<DomainPost> repo, IHttpContextAccessor http)
        {
            _repo = repo;
            _http = http;
        }

        public async Task<PagedResult<PostListItemViewModel>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 || request.PageSize > 100 ? 20 : request.PageSize;

            var query = _repo.AsQueryable().Where(p => !p.isDeleted);

            // User scope: only posts by current user (if authenticated)
            var userIdStr = _http?.HttpContext?.Items?["UserId"] as string ?? _http?.HttpContext?.User?.FindFirst("nameid")?.Value;
            if (!string.IsNullOrWhiteSpace(userIdStr) && Guid.TryParse(userIdStr, out var userId))
            {
                query = query.Where(p => p.AuthorId == userId);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
                query = query.Where(p => p.Title.Contains(request.Search));

            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(p => p.Status == request.Status);

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(p => p.PublishedAt ?? p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PostListItemViewModel
                {
                    ID = p.ID,
                    Title = p.Title,
                    Excerpt = p.Excerpt,
                    Status = p.Status,
                    CreatedDate = p.CreatedDate,
                    PublishedAt = p.PublishedAt
                })
                .ToArrayAsync(cancellationToken);

            return new PagedResult<PostListItemViewModel>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = items
            };
        }
    }
}

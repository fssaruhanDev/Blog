using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Api.Application.Features.Queries.Post.GetPosts;
using Blog.Api.Domain.Interfaces.Repositories;
using DomainPost = Blog.Api.Domain.Models.Post;
using Blog.Common.Models.Queries;
using Blog.Common.Models.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Blog.Common.Infrastructure;

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
            if (UserContextHelper.TryGetUserId(_http, out var userId))
                query = query.Where(p => p.AuthorId == userId);

            if (!string.IsNullOrWhiteSpace(request.Search))
                query = query.Where(p => p.Title.Contains(request.Search));

            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(p => p.Status == request.Status);

            var total = await query.CountAsync(cancellationToken);

            var baseUrl = _http.HttpContext != null ? $"{_http.HttpContext.Request.Scheme}://{_http.HttpContext.Request.Host}" : string.Empty;

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
                    PublishedAt = p.PublishedAt,
                    CoverImageUrl = string.IsNullOrWhiteSpace(p.CoverImageUrl) ? null :
                        (p.CoverImageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                            ? p.CoverImageUrl
                            : ($"{baseUrl}{(p.CoverImageUrl.StartsWith("/")?"":"/")}{p.CoverImageUrl}"))
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

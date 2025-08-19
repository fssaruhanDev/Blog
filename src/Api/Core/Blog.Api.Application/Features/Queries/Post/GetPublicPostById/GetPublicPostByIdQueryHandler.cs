using System.Threading;
using System.Threading.Tasks;
using Blog.Api.Application.Interfaces.Repostrories;
using DomainPost = Blog.Api.Domain.Models.Post;
using Blog.Common.Models.Queries.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Blog.Api.Application.Features.Queries.Post.GetPublicPostById
{
    public class GetPublicPostByIdQueryHandler : IRequestHandler<GetPublicPostByIdQuery, PostDetailViewModel?>
    {
        private readonly IGenericRepository<DomainPost> _repo;
        private readonly IHttpContextAccessor _http;
        
        public GetPublicPostByIdQueryHandler(IGenericRepository<DomainPost> repo, IHttpContextAccessor http)
        {
            _repo = repo;
            _http = http;
        }

        public async Task<PostDetailViewModel?> Handle(GetPublicPostByIdQuery request, CancellationToken cancellationToken)
        {
            var baseUrl = _http.HttpContext != null ? $"{_http.HttpContext.Request.Scheme}://{_http.HttpContext.Request.Host}" : string.Empty;

            // Public query: only published posts, no user scoping
            var post = await _repo.AsQueryable()
                .Where(p => p.ID == request.Id && !p.isDeleted && p.Status == "published")
                .Select(p => new PostDetailViewModel
                {
                    ID = p.ID,
                    Title = p.Title,
                    Excerpt = p.Excerpt,
                    Content = p.Content,
                    Status = p.Status,
                    CreatedDate = p.CreatedDate,
                    PublishedAt = p.PublishedAt,
                    IsFeatured = p.IsFeatured,
                    CoverImageUrl = string.IsNullOrWhiteSpace(p.CoverImageUrl) ? null :
                        (p.CoverImageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                            ? p.CoverImageUrl
                            : ($"{baseUrl}{(p.CoverImageUrl.StartsWith("/")?"":"/")}{p.CoverImageUrl}"))
                })
                .FirstOrDefaultAsync(cancellationToken);

            return post;
        }
    }
}
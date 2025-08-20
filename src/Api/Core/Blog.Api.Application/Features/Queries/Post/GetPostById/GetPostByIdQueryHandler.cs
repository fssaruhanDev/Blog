using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries.Post;
using MediatR;
using DomainPost = Blog.Api.Domain.Models.Post;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Blog.Common.Infrastructure.Exeptions;

namespace Blog.Api.Application.Features.Queries.Post.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDetailViewModel>
    {
        private readonly IGenericRepository<DomainPost> _repo;
        private readonly IHttpContextAccessor _http;
        public GetPostByIdQueryHandler(IGenericRepository<DomainPost> repo, IHttpContextAccessor http)
        { _repo = repo; _http = http; }

        public async Task<PostDetailViewModel> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var p = await _repo.GetByIdAsync(request.Id) ?? throw new NotFoundException("Post not found");
            var baseUrl = _http.HttpContext != null ? $"{_http.HttpContext.Request.Scheme}://{_http.HttpContext.Request.Host}" : string.Empty;
            return new PostDetailViewModel
            {
                ID = p.ID,
                Title = p.Title,
                Excerpt = p.Excerpt,
                Content = p.Content,
                Status = p.Status,
                CreatedDate = p.CreatedDate,
                PublishedAt = p.PublishedAt,
                CoverImageUrl = string.IsNullOrWhiteSpace(p.CoverImageUrl) ? null :
                    (p.CoverImageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                        ? p.CoverImageUrl
                        : ($"{baseUrl}{(p.CoverImageUrl.StartsWith("/")?"":"/")}{p.CoverImageUrl}"))
            };
        }
    }
}

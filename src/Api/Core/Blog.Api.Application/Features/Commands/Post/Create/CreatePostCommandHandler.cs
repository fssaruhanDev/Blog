using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Common.Models.Queries.Post;
using Blog.Common.Models.RequestModels.Post;
using MediatR;
using DomainPost = Blog.Api.Domain.Models.Post;
using Microsoft.AspNetCore.Http;
using Blog.Common.Infrastructure;

namespace Blog.Api.Application.Features.Commands.Post.Create
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostListItemViewModel>
    {
        private readonly IGenericRepository<DomainPost> _repo;
        private readonly IHttpContextAccessor _http;
        public CreatePostCommandHandler(IGenericRepository<DomainPost> repo, IHttpContextAccessor http)
        {
            _repo = repo; _http = http;
        }

        public async Task<PostListItemViewModel> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            if (!UserContextHelper.TryGetUserId(_http, out var userId))
                throw new UnauthorizedAccessException();

            var entity = new DomainPost
            {
                ID = Guid.NewGuid(),
                AuthorId = userId,
                Title = request.Title,
                Excerpt = request.Excerpt,
                Content = request.Content,
                Status = request.Status ?? "draft",
                PublishedAt = request.PublishedAt,
                CoverImageUrl = request.CoverImageUrl,
                CreatedDate = DateTime.UtcNow,
                isDeleted = false
            };

            await _repo.AddAsync(entity);

            var baseUrl = _http.HttpContext != null ? $"{_http.HttpContext.Request.Scheme}://{_http.HttpContext.Request.Host}" : string.Empty;
            return new PostListItemViewModel
            {
                ID = entity.ID,
                Title = entity.Title,
                Excerpt = entity.Excerpt,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate,
                PublishedAt = entity.PublishedAt,
                CoverImageUrl = string.IsNullOrWhiteSpace(entity.CoverImageUrl) ? null :
                    (entity.CoverImageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                        ? entity.CoverImageUrl
                        : ($"{baseUrl}{(entity.CoverImageUrl.StartsWith("/")?"":"/")}{entity.CoverImageUrl}"))
            };
        }
    }
}

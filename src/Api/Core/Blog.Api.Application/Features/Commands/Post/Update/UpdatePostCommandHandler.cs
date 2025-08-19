using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries.Post;
using Blog.Common.Models.RequestModels.Post;
using MediatR;
using DomainPost = Blog.Api.Domain.Models.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Blog.Common.Infrastructure;

namespace Blog.Api.Application.Features.Commands.Post.Update
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostListItemViewModel>
    {
        private readonly IGenericRepository<DomainPost> _repo;
        private readonly IHttpContextAccessor _http;
        public UpdatePostCommandHandler(IGenericRepository<DomainPost> repo, IHttpContextAccessor http)
        {
            _repo = repo; _http = http;
        }

        public async Task<PostListItemViewModel> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            if (!UserContextHelper.TryGetUserId(_http, out var userId))
                throw new UnauthorizedAccessException();

            var entity = await _repo.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException("Post not found");
            if (entity.AuthorId != userId) throw new UnauthorizedAccessException();

            entity.Title = request.Title;
            entity.Excerpt = request.Excerpt;
            entity.Content = request.Content;
            entity.Status = request.Status ?? entity.Status;
            entity.PublishedAt = request.PublishedAt;
            // Only update cover if caller explicitly sends a non-empty value.
            // (Empty string from old UI should NOT wipe existing cover.)
            if (request.CoverImageUrl != null)
            {
                var trimmed = request.CoverImageUrl.Trim();
                if (!string.IsNullOrWhiteSpace(trimmed))
                    entity.CoverImageUrl = trimmed;
                // else: ignore empty => keep previous (if explicit remove is needed, introduce a separate flag later)
            }

            await _repo.UpdateAsync(entity);

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

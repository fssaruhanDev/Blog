using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Common.Models.Queries.Post;
using Blog.Common.Models.RequestModels.Post;
using MediatR;
using DomainPost = Blog.Api.Domain.Models.Post;
using Microsoft.AspNetCore.Http;

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
            var userIdStr = _http?.HttpContext?.Items?["UserId"] as string ?? _http?.HttpContext?.User?.FindFirst("nameid")?.Value;
            if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
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
                CreatedDate = DateTime.UtcNow,
                isDeleted = false
            };

            await _repo.AddAsync(entity);

            return new PostListItemViewModel
            {
                ID = entity.ID,
                Title = entity.Title,
                Excerpt = entity.Excerpt,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate,
                PublishedAt = entity.PublishedAt
            };
        }
    }
}

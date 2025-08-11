using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Common.Models.Queries.Post;
using Blog.Common.Models.RequestModels.Post;
using MediatR;
using DomainPost = Blog.Api.Domain.Models.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
            var userIdStr = _http?.HttpContext?.Items?["UserId"] as string ?? _http?.HttpContext?.User?.FindFirst("nameid")?.Value;
            if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                throw new UnauthorizedAccessException();

            var entity = await _repo.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException("Post not found");
            if (entity.AuthorId != userId) throw new UnauthorizedAccessException();

            entity.Title = request.Title;
            entity.Excerpt = request.Excerpt;
            entity.Content = request.Content;
            entity.Status = request.Status ?? entity.Status;
            entity.PublishedAt = request.PublishedAt;

            await _repo.UpdateAsync(entity);

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

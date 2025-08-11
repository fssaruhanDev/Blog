using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Common.Models.RequestModels.Post;
using MediatR;
using DomainPost = Blog.Api.Domain.Models.Post;
using Microsoft.AspNetCore.Http;

namespace Blog.Api.Application.Features.Commands.Post.Delete
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, bool>
    {
        private readonly IGenericRepository<DomainPost> _repo;
        private readonly IHttpContextAccessor _http;
        public DeletePostCommandHandler(IGenericRepository<DomainPost> repo, IHttpContextAccessor http)
        {
            _repo = repo; _http = http;
        }

        public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var userIdStr = _http?.HttpContext?.Items?["UserId"] as string ?? _http?.HttpContext?.User?.FindFirst("nameid")?.Value;
            if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                throw new UnauthorizedAccessException();

            var entity = await _repo.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException("Post not found");
            if (entity.AuthorId != userId) throw new UnauthorizedAccessException();

            await _repo.DeleteAsync(entity);
            return true;
        }
    }
}

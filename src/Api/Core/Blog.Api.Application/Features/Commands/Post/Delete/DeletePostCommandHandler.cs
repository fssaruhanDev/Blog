using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.RequestModels.Post;
using MediatR;
using DomainPost = Blog.Api.Domain.Models.Post;
using Microsoft.AspNetCore.Http;
using Blog.Common.Infrastructure;

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
            if (!UserContextHelper.TryGetUserId(_http, out var userId))
                throw new UnauthorizedAccessException();

            var entity = await _repo.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException("Post not found");
            if (entity.AuthorId != userId) throw new UnauthorizedAccessException();

            await _repo.DeleteAsync(entity);
            return true;
        }
    }
}

using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.RequestModels.News;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DomainNews = Blog.Api.Domain.Models.News;

namespace Blog.Api.Application.Features.Commands.News.Delete
{
    public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand, bool>
    {
        private readonly IGenericRepository<DomainNews> _repo;
        public DeleteNewsCommandHandler(IGenericRepository<DomainNews> repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.AsQueryable().FirstOrDefaultAsync(x => x.ID == request.Id, cancellationToken);
            if (entity == null)
                return false;

            // Soft delete
            entity.isDeleted = true;
            await _repo.UpdateAsync(entity);
            return true;
        }
    }
}

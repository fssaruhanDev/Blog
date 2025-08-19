using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.RequestModels.Pages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DomainPage = Blog.Api.Domain.Models.Page;

namespace Blog.Api.Application.Features.Commands.Pages.Delete
{
    public class DeletePageCommandHandler : IRequestHandler<DeletePageCommand, bool>
    {
        private readonly IGenericRepository<DomainPage> _repo;
        public DeletePageCommandHandler(IGenericRepository<DomainPage> repo)
        { _repo = repo; }

        public async Task<bool> Handle(DeletePageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.AsQueryable().FirstOrDefaultAsync(x => x.ID == request.Id && !x.isDeleted, cancellationToken);
            if (entity == null) return false;
            entity.isDeleted = true;
            await _repo.UpdateAsync(entity);
            return true;
        }
    }
}

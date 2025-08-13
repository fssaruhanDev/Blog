using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Common.Models.Queries.Pages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DomainPage = Blog.Api.Domain.Models.Page;

namespace Blog.Api.Application.Features.Queries.Pages.GetPageById
{
    public class GetPageByIdQueryHandler : IRequestHandler<GetPageByIdQuery, PageListItemViewModel>
    {
        private readonly IGenericRepository<DomainPage> _repo;
        public GetPageByIdQueryHandler(IGenericRepository<DomainPage> repo) { _repo = repo; }

        public async Task<PageListItemViewModel> Handle(GetPageByIdQuery request, CancellationToken cancellationToken)
        {
            var query = _repo.AsQueryable().Where(p => !p.isDeleted && p.ID == request.Id);
            if (request.PublicOnly)
                query = query.Where(p => p.Status == "published");
            var p = await query.FirstOrDefaultAsync(cancellationToken);
            if (p == null) return null!;
            return new PageListItemViewModel
            {
                ID = p.ID,
                Slug = p.Slug,
                Title = p.Title,
                Excerpt = p.Excerpt,
                Status = p.Status,
                CreatedDate = p.CreatedDate,
                PublishedAt = p.PublishedAt
            };
        }
    }
}

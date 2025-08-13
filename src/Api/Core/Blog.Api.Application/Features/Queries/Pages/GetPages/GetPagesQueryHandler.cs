using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Common.Models.Queries;
using Blog.Common.Models.Queries.Pages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DomainPage = Blog.Api.Domain.Models.Page;

namespace Blog.Api.Application.Features.Queries.Pages.GetPages
{
    public class GetPagesQueryHandler : IRequestHandler<GetPagesQuery, PagedResult<PageListItemViewModel>>
    {
        private readonly IGenericRepository<DomainPage> _repo;
        public GetPagesQueryHandler(IGenericRepository<DomainPage> repo) { _repo = repo; }

        public async Task<PagedResult<PageListItemViewModel>> Handle(GetPagesQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 || request.PageSize > 100 ? 20 : request.PageSize;
            var query = _repo.AsQueryable().Where(p => !p.isDeleted);
            if (request.PublicOnly)
                query = query.Where(p => p.Status == "published");
            if (!string.IsNullOrWhiteSpace(request.Search))
                query = query.Where(p => p.Title.Contains(request.Search) || p.Slug.Contains(request.Search));
            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(p => p.Status == request.Status);

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(p => p.PublishedAt ?? p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PageListItemViewModel
                {
                    ID = p.ID,
                    Slug = p.Slug,
                    Title = p.Title,
                    Excerpt = p.Excerpt,
                    Status = p.Status,
                    CreatedDate = p.CreatedDate,
                    PublishedAt = p.PublishedAt
                }).ToArrayAsync(cancellationToken);

            return new PagedResult<PageListItemViewModel>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = items
            };
        }
    }
}

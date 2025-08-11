using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Common.Models.Queries;
using Blog.Common.Models.Queries.News;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using DomainNews = Blog.Api.Domain.Models.News;

namespace Blog.Api.Application.Features.Queries.News.GetNews
{
    public class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, PagedResult<NewsListItemViewModel>>
    {
        private readonly IGenericRepository<DomainNews> _repo;
        private readonly IHttpContextAccessor _http;
        public GetNewsQueryHandler(IGenericRepository<DomainNews> repo, IHttpContextAccessor http)
        {
            _repo = repo; _http = http;
        }

        public async Task<PagedResult<NewsListItemViewModel>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 || request.PageSize > 100 ? 20 : request.PageSize;

            var query = _repo.AsQueryable().Where(n => !n.isDeleted);

            if (request.PublicOnly)
                query = query.Where(n => n.Status == "published");

            if (!string.IsNullOrWhiteSpace(request.Search))
                query = query.Where(n => n.Title.Contains(request.Search) || (n.Summary ?? "").Contains(request.Search));

            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(n => n.Status == request.Status);

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(n => n.PublishedAt ?? n.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new NewsListItemViewModel
                {
                    ID = n.ID,
                    Title = n.Title,
                    Summary = n.Summary,
                    Status = n.Status,
                    CreatedDate = n.CreatedDate,
                    PublishedAt = n.PublishedAt,
                    SourceName = n.SourceName,
                    SourceUrl = n.SourceUrl,
                    Tags = n.Tags
                })
                .ToArrayAsync(cancellationToken);

            return new PagedResult<NewsListItemViewModel>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = items
            };
        }
    }
}

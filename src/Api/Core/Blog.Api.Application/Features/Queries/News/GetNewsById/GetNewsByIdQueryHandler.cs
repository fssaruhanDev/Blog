using System.Threading;
using System.Threading.Tasks;
using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Common.Models.Queries.News;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DomainNews = Blog.Api.Domain.Models.News;

namespace Blog.Api.Application.Features.Queries.News.GetNewsById
{
    public class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, NewsListItemViewModel>
    {
        private readonly IGenericRepository<DomainNews> _repo;
        public GetNewsByIdQueryHandler(IGenericRepository<DomainNews> repo)
        {
            _repo = repo;
        }

        public async Task<NewsListItemViewModel> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
        {
            var query = _repo.AsQueryable().Where(n => !n.isDeleted && n.ID == request.Id);
            if (request.PublicOnly)
                query = query.Where(n => n.Status == "published");

            var n = await query.FirstOrDefaultAsync(cancellationToken);
            if (n == null) return null!;

            return new NewsListItemViewModel
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
            };
        }
    }
}

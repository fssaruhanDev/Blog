using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Common.Models.Queries.News;
using Blog.Common.Models.RequestModels.News;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DomainNews = Blog.Api.Domain.Models.News;

namespace Blog.Api.Application.Features.Commands.News.Update
{
    public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, NewsListItemViewModel>
    {
        private readonly IGenericRepository<DomainNews> _repo;
        public UpdateNewsCommandHandler(IGenericRepository<DomainNews> repo)
        {
            _repo = repo;
        }

        public async Task<NewsListItemViewModel> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.AsQueryable().FirstOrDefaultAsync(x => x.ID == request.Id, cancellationToken);
            if (entity == null)
                throw new KeyNotFoundException("News bulunamadı");

            // Alan güncellemeleri
            if (!string.IsNullOrWhiteSpace(request.Title))
                entity.Title = request.Title.Trim();

            if (request.Summary != null)
                entity.Summary = request.Summary; // boş string gönderilirse temizlenebilir

            if (request.SourceName != null)
                entity.SourceName = request.SourceName;

            if (request.SourceUrl != null)
                entity.SourceUrl = request.SourceUrl;

            if (request.Status != null)
                entity.Status = request.Status;

            entity.PublishedAt = request.PublishedAt ?? entity.PublishedAt;
            if (request.Tags != null)
                entity.Tags = request.Tags; // boş string gönderilirse temizlenebilir

            await _repo.UpdateAsync(entity);

            return new NewsListItemViewModel
            {
                ID = entity.ID,
                Title = entity.Title,
                Summary = entity.Summary,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate,
                PublishedAt = entity.PublishedAt,
                SourceName = entity.SourceName,
                SourceUrl = entity.SourceUrl,
                Tags = entity.Tags
            };
        }
    }
}

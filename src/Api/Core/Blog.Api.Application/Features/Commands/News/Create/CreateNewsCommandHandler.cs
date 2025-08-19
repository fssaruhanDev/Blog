using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries.News;
using Blog.Common.Models.RequestModels.News;
using MediatR;
using Microsoft.EntityFrameworkCore; // AnyAsync
using DomainNews = Blog.Api.Domain.Models.News;

namespace Blog.Api.Application.Features.Commands.News.Create
{
    public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, NewsListItemViewModel>
    {
        private readonly IGenericRepository<DomainNews> _repo;
        public CreateNewsCommandHandler(IGenericRepository<DomainNews> repo)
        {
            _repo = repo;
        }

        public async Task<NewsListItemViewModel> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            // Basit alan validasyonu
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title gereklidir");

            // SourceUrl varsa duplication'ı DB exception'a düşmeden önce kontrol et
            if (!string.IsNullOrWhiteSpace(request.SourceUrl))
            {
                var normalized = NormalizeUrl(request.SourceUrl!);
                // Aynı normalized url var mı?
                var exists = await _repo.AsQueryable()
                    .AnyAsync(n => !n.isDeleted && n.SourceUrl != null && n.SourceUrl == normalized, cancellationToken);
                if (exists)
                    throw new InvalidOperationException("Bu SourceUrl zaten eklenmiş.");
                request.SourceUrl = normalized; // normalize edilmiş halini kaydet
            }

            var entity = new DomainNews
            {
                ID = Guid.NewGuid(),
                Title = request.Title.Trim(),
                Summary = request.Summary,
                SourceName = request.SourceName,
                SourceUrl = request.SourceUrl,
                Status = string.IsNullOrWhiteSpace(request.Status) ? "draft" : request.Status!,
                PublishedAt = request.PublishedAt,
                Tags = request.Tags,
                CreatedDate = DateTime.UtcNow,
                isDeleted = false
            };

            await _repo.AddAsync(entity);

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
        private static string NormalizeUrl(string url)
        {
            url = url.Trim();
            // trailing slash (domain root hariç) kaldır
            if (url.Length > 1 && url.EndsWith('/')) url = url.TrimEnd('/');
            // Lower-case host kısmını basitçe tümünü lower-case yapıyoruz (case insensitive)
            url = url.ToLowerInvariant();
            return url;
        }
    }
}

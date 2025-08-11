using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Common.Models.Queries.News;
using Blog.Common.Models.RequestModels.News;
using MediatR;
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
            // UNIQUE SourceUrl kontrolünü DB yapacak; burada basic alan validasyonu
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title gereklidir");

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
    }
}

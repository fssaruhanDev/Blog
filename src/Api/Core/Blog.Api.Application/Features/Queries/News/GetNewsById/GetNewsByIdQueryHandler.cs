using System.Threading;
using System.Threading.Tasks;
using Blog.Api.Application.Interfaces.Infrastructure.Utility.Logger;
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries.News;
using MediatR;
using System.Collections.Generic;

namespace Blog.Api.Application.Features.Queries.News.GetNewsById
{
    public class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, NewsDetailViewModel>
    {
        private readonly INewsRepository _newsRepository;
        private readonly ILoggerService _loggerService;
        
        public GetNewsByIdQueryHandler(INewsRepository newsRepository, ILoggerService loggerService)
        {
            _newsRepository = newsRepository;
            _loggerService = loggerService;
        }

        public async Task<NewsDetailViewModel> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
        {
            var logProps = new Dictionary<string, object>
            {
                ["NewsId"] = request.Id,
                ["PublicOnly"] = request.PublicOnly
            };

            _loggerService.LogInformation("Fetching news by ID.", logProps);

            var news = await _newsRepository.GetByIdAsync(request.Id);
            
            if (news == null || news.isDeleted)
            {
                _loggerService.LogWarning("News not found.", logProps);
                return null!;
            }

            // Check public access restriction
            if (request.PublicOnly && news.Status != "published")
            {
                _loggerService.LogWarning("News access denied: not published.", logProps);
                return null!;
            }

            _loggerService.LogInformation("News fetched successfully.", logProps);

            return new NewsDetailViewModel
            {
                Id = news.ID,
                Title = news.Title,
                Summary = news.Summary,
                Status = news.Status,
                CreatedAt = news.CreatedDate,
                UpdatedAt = news.UpdatedDate ?? news.CreatedDate,
                PublishedAt = news.PublishedAt,
                SourceName = news.SourceName,
                SourceUrl = news.SourceUrl,
                Tags = news.Tags
            };
        }
    }
}

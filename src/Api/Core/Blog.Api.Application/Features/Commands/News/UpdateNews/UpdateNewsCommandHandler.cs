using Blog.Api.Application.Interfaces.Infrastructure.Utility.Logger;
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries.News;
using Blog.Api.Application.Features.Commands.News.UpdateNews;
using MediatR;
using System;
using Blog.Common.Infrastructure.Exeptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Api.Application.Features.Commands.News.UpdateNews
{
    public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, NewsDetailViewModel>
    {
        private readonly INewsRepository _newsRepository;
        private readonly ILoggerService _loggerService;

        public UpdateNewsCommandHandler(INewsRepository newsRepository, ILoggerService loggerService)
        {
            _newsRepository = newsRepository;
            _loggerService = loggerService;
        }

        public async Task<NewsDetailViewModel> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var logProps = new Dictionary<string, object>
            {
                ["NewsId"] = request.Id,
                ["Title"] = request.Title
            };

            _loggerService.LogInformation("Updating news.", logProps);

            var news = await _newsRepository.GetByIdAsync(request.Id);
            if (news == null)
            {
                _loggerService.LogWarning("News update failed: News not found.", logProps);
                throw new NotFoundException("News not found.");
            }

            // Check if title is unique (exclude current news)
            if (news.Title != request.Title)
            {
                var isTitleUnique = await _newsRepository.IsTitleUniqueAsync(request.Title, request.Id);
                    if (!isTitleUnique)
                    {
                        _loggerService.LogWarning("News update failed: Title already exists.", logProps);
                        throw new BadRequestException("A news with this title already exists.");
                    }
            }

            // Update properties
            news.Title = request.Title;
            news.Summary = request.Summary;
            news.SourceName = request.SourceName;
            news.SourceUrl = request.SourceUrl;
            news.Status = request.Status;
            news.PublishedAt = request.PublishedAt;
            news.Tags = request.Tags;
            news.UpdatedDate = DateTime.UtcNow;
            news.isModified = true;

            await _newsRepository.UpdateAsync(news);

            _loggerService.LogInformation("News updated successfully.", logProps);

            return new NewsDetailViewModel
            {
                Id = news.ID,
                Title = news.Title,
                Summary = news.Summary,
                SourceName = news.SourceName,
                SourceUrl = news.SourceUrl,
                Status = news.Status,
                PublishedAt = news.PublishedAt,
                Tags = news.Tags,
                CreatedAt = news.CreatedDate,
                UpdatedAt = news.UpdatedDate ?? news.CreatedDate
            };
        }
    }
}

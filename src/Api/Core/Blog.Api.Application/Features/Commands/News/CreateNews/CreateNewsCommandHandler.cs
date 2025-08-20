using Blog.Api.Application.Interfaces.Infrastructure.Utility.Logger;
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;
using Blog.Common.Models.Queries.News;
using Blog.Api.Application.Features.Commands.News.CreateNews;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Api.Application.Features.Commands.News.CreateNews
{
    public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, NewsDetailViewModel>
    {
        private readonly INewsRepository _newsRepository;
        private readonly ILoggerService _loggerService;

        public CreateNewsCommandHandler(INewsRepository newsRepository, ILoggerService loggerService)
        {
            _newsRepository = newsRepository;
            _loggerService = loggerService;
        }

        public async Task<NewsDetailViewModel> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            var logProps = new Dictionary<string, object>
            {
                ["Title"] = request.Title
            };

            _loggerService.LogInformation("Creating new news.", logProps);

            // Check if title is unique
            var isTitleUnique = await _newsRepository.IsTitleUniqueAsync(request.Title);
            if (!isTitleUnique)
            {
                _loggerService.LogWarning("News creation failed: Title already exists.", logProps);
                throw new Blog.Common.Infrastructure.Exeptions.BadRequestException("A news with this title already exists.");
            }

            var news = new Blog.Api.Domain.Models.News
            {
                ID = Blog.Common.Helpers.UlidHelper.NewGuid(),
                Title = request.Title,
                Summary = request.Summary,
                SourceName = request.SourceName,
                SourceUrl = request.SourceUrl,
                Status = request.Status,
                PublishedAt = request.PublishedAt,
                Tags = request.Tags,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = Guid.Empty, // TODO: Get from current user
                isActive = true,
                isDeleted = false,
                isModified = false
            };

            await _newsRepository.AddAsync(news);

            logProps["NewsId"] = news.ID;
            _loggerService.LogInformation("News created successfully.", logProps);

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

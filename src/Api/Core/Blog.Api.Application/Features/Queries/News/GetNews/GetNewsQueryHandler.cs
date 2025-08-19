using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Api.Application.Interfaces.Infrastructure.Utility.Logger;
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries;
using Blog.Common.Models.Queries.News;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Blog.Api.Application.Features.Queries.News.GetNews
{
    public class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, PagedResult<NewsListItemViewModel>>
    {
        private readonly INewsRepository _newsRepository;
        private readonly ILoggerService _loggerService;
        
        public GetNewsQueryHandler(INewsRepository newsRepository, ILoggerService loggerService)
        {
            _newsRepository = newsRepository;
            _loggerService = loggerService;
        }

        public async Task<PagedResult<NewsListItemViewModel>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
        {
            var logProps = new Dictionary<string, object>
            {
                ["Page"] = request.Page,
                ["PageSize"] = request.PageSize,
                ["Status"] = request.Status ?? "all",
                ["Search"] = request.Search ?? "none"
            };

            _loggerService.LogInformation("Fetching news list.", logProps);

            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 || request.PageSize > 100 ? 20 : request.PageSize;

            List<Blog.Api.Domain.Models.News> items;
            
            // Use specific repository methods based on request
            if (request.PublicOnly)
            {
                items = await _newsRepository.GetPublishedNewsAsync(page, pageSize);
            }
            else if (!string.IsNullOrWhiteSpace(request.Status))
            {
                items = await _newsRepository.GetNewsByStatusAsync(request.Status, page, pageSize);
            }
            else if (!string.IsNullOrWhiteSpace(request.Search))
            {
                items = await _newsRepository.SearchNewsAsync(request.Search, page, pageSize);
            }
            else
            {
                // Get all with pagination using generic repository
                items = await _newsRepository.GetList(
                    predicate: n => !n.isDeleted,
                    noTracking: true,
                    orderBy: q => q.OrderByDescending(n => n.PublishedAt ?? n.CreatedDate)
                );
                
                // Manual pagination for complex query
                items = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }

            // Get total count for pagination
            var totalQuery = _newsRepository.AsQueryable().Where(n => !n.isDeleted);
            
            if (request.PublicOnly)
                totalQuery = totalQuery.Where(n => n.Status == "published");
            if (!string.IsNullOrWhiteSpace(request.Status))
                totalQuery = totalQuery.Where(n => n.Status == request.Status);
            if (!string.IsNullOrWhiteSpace(request.Search))
                totalQuery = totalQuery.Where(n => n.Title.Contains(request.Search) || (n.Summary ?? "").Contains(request.Search));

            var total = await totalQuery.CountAsync(cancellationToken);

            var viewModels = items.Select(n => new NewsListItemViewModel
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
            }).ToArray();

            logProps["TotalCount"] = total;
            logProps["ReturnedCount"] = viewModels.Length;
            _loggerService.LogInformation("News list fetched successfully.", logProps);

            return new PagedResult<NewsListItemViewModel>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = viewModels
            };
        }
    }
}

using System;
using Blog.Common.Models.Queries.News;
using MediatR;

namespace Blog.Common.Models.RequestModels.News
{
    public class UpdateNewsCommand : IRequest<NewsListItemViewModel>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? SourceName { get; set; }
        public string? SourceUrl { get; set; }
        public string? Status { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? Tags { get; set; }
    }
}

using Blog.Common.Models.Queries.News;
using MediatR;

namespace Blog.Api.Application.Features.Commands.News.UpdateNews
{
    public class UpdateNewsCommand : IRequest<NewsDetailViewModel>
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Summary { get; set; }
        public string? SourceName { get; set; }
        public string? SourceUrl { get; set; }
        public string Status { get; set; } = "draft";
        public DateTime? PublishedAt { get; set; }
        public string? Tags { get; set; }
    }
}

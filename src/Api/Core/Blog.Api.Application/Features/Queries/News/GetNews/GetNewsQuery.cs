using Blog.Common.Models.Queries;
using Blog.Common.Models.Queries.News;
using MediatR;

namespace Blog.Api.Application.Features.Queries.News.GetNews
{
    public class GetNewsQuery : IRequest<PagedResult<NewsListItemViewModel>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
        public string? Status { get; set; }
        public bool PublicOnly { get; set; } = false;
    }
}

using Blog.Common.Models.Queries;
using Blog.Common.Models.Queries.Post;
using MediatR;

namespace Blog.Api.Application.Features.Queries.Post.GetPosts
{
    public class GetPostsQuery : IRequest<PagedResult<PostListItemViewModel>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
        public string? Status { get; set; }
    }
}

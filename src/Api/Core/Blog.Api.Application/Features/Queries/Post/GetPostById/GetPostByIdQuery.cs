using Blog.Common.Models.Queries.Post;
using MediatR;

namespace Blog.Api.Application.Features.Queries.Post.GetPostById
{
    public class GetPostByIdQuery : IRequest<PostDetailViewModel>
    {
        public Guid Id { get; set; }
    }
}

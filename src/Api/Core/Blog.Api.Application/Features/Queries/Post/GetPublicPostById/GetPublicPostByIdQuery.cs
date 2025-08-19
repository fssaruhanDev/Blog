using Blog.Common.Models.Queries.Post;
using MediatR;

namespace Blog.Api.Application.Features.Queries.Post.GetPublicPostById
{
    public class GetPublicPostByIdQuery : IRequest<PostDetailViewModel?>
    {
        public Guid Id { get; set; }
    }
}
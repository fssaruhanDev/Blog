using Blog.Common.Models.RequestModels.Post;
using Blog.Common.Models.Queries.Post;
using MediatR;

namespace Blog.Api.Application.Features.Commands.Post.CreatePost
{
    public class CreatePostCommand : IRequest<PostDetailViewModel>
    {
        public Guid AuthorId { get; set; }
        public required string Title { get; set; }
        public string? Excerpt { get; set; }
        public string? Content { get; set; }
        public string Status { get; set; } = "draft";
        public DateTime? PublishedAt { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public bool IsFeatured { get; set; }
        public string? CoverImageUrl { get; set; }
    }
}

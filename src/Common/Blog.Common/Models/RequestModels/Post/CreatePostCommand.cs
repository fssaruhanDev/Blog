using System;
using Blog.Common.Models.Queries.Post;
using MediatR;

namespace Blog.Common.Models.RequestModels.Post
{
    public class CreatePostCommand : IRequest<PostListItemViewModel>
    {
        public string Title { get; set; } = string.Empty;
        public string? Excerpt { get; set; }
        public string? Content { get; set; }
        public string Status { get; set; } = "draft"; // draft|published|scheduled
        public DateTime? PublishedAt { get; set; }
    public string? CoverImageUrl { get; set; }
    }
}

using Blog.Api.Application.Interfaces.Infrastructure.Utility.Logger;
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;
using Blog.Common.Models.Queries.Post;
using Blog.Api.Application.Features.Commands.Post.CreatePost;
using MediatR;
using System;
using Blog.Common.Infrastructure.Exeptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Api.Application.Features.Commands.Post.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDetailViewModel>
    {
        private readonly IPostRepository _postRepository;
        private readonly ILoggerService _loggerService;

        public CreatePostCommandHandler(IPostRepository postRepository, ILoggerService loggerService)
        {
            _postRepository = postRepository;
            _loggerService = loggerService;
        }

        public async Task<PostDetailViewModel> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var logProps = new Dictionary<string, object>
            {
                ["Title"] = request.Title,
                ["AuthorId"] = request.AuthorId
            };

            _loggerService.LogInformation("Creating new post.", logProps);

            // Check if title is unique
            var isTitleUnique = await _postRepository.IsTitleUniqueAsync(request.Title);
            if (!isTitleUnique)
            {
                _loggerService.LogWarning("Post creation failed: Title already exists.", logProps);
                throw new BadRequestException("A post with this title already exists.");
            }

            var post = new Blog.Api.Domain.Models.Post
            {
                ID = Blog.Common.Helpers.UlidHelper.NewGuid(),
                AuthorId = request.AuthorId,
                Title = request.Title,
                Excerpt = request.Excerpt,
                Content = request.Content,
                Status = request.Status,
                PublishedAt = request.PublishedAt,
                ScheduledAt = request.ScheduledAt,
                IsFeatured = request.IsFeatured,
                CoverImageUrl = request.CoverImageUrl,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = request.AuthorId,
                isActive = true,
                isDeleted = false,
                isModified = false
            };

            // Calculate reading time if content exists
            if (!string.IsNullOrEmpty(request.Content))
            {
                var wordCount = request.Content.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                post.ReadingTime = Math.Max(1, wordCount / 200); // Average reading speed: 200 words per minute
            }

            await _postRepository.AddAsync(post);

            logProps["PostId"] = post.ID;
            _loggerService.LogInformation("Post created successfully.", logProps);

            return new PostDetailViewModel
            {
                ID = post.ID,
                Title = post.Title,
                Excerpt = post.Excerpt,
                Content = post.Content,
                Status = post.Status,
                CreatedDate = post.CreatedDate,
                PublishedAt = post.PublishedAt,
                IsFeatured = post.IsFeatured,
                CoverImageUrl = post.CoverImageUrl
            };
        }
    }
}

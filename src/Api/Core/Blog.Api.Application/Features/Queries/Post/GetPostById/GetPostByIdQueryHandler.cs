using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Common.Models.Queries.Post;
using MediatR;
using DomainPost = Blog.Api.Domain.Models.Post;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Application.Features.Queries.Post.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDetailViewModel>
    {
        private readonly IGenericRepository<DomainPost> _repo;
        public GetPostByIdQueryHandler(IGenericRepository<DomainPost> repo)
        { _repo = repo; }

        public async Task<PostDetailViewModel> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var p = await _repo.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException();
            return new PostDetailViewModel
            {
                ID = p.ID,
                Title = p.Title,
                Excerpt = p.Excerpt,
                Content = p.Content,
                Status = p.Status,
                CreatedDate = p.CreatedDate,
                PublishedAt = p.PublishedAt
            };
        }
    }
}

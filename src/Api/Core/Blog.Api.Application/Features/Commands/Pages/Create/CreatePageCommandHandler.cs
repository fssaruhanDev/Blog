using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Common.Models.RequestModels.Pages;
using Blog.Common.Models.Queries.Pages;
using MediatR;
using DomainPage = Blog.Api.Domain.Models.Page;

namespace Blog.Api.Application.Features.Commands.Pages.Create
{
    public class CreatePageCommandHandler : IRequestHandler<CreatePageCommand, PageListItemViewModel>
    {
        private readonly IGenericRepository<DomainPage> _repo;
        public CreatePageCommandHandler(IGenericRepository<DomainPage> repo)
        { _repo = repo; }

        public async Task<PageListItemViewModel> Handle(CreatePageCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Title)) throw new ArgumentException("Title required");
            if (string.IsNullOrWhiteSpace(request.Slug))
                request.Slug = request.Title.ToLower().Replace(' ', '-');

            var entity = new DomainPage
            {
                ID = Guid.NewGuid(),
                Slug = request.Slug.Trim(),
                Title = request.Title.Trim(),
                Excerpt = request.Excerpt,
                Content = request.Content,
                Status = string.IsNullOrWhiteSpace(request.Status) ? "draft" : request.Status!,
                PublishedAt = request.PublishedAt,
                IsStandalone = request.IsStandalone,
                MetaTitle = request.MetaTitle,
                MetaDescription = request.MetaDescription,
                CreatedDate = DateTime.UtcNow,
                isDeleted = false
            };
            await _repo.AddAsync(entity);

            return new PageListItemViewModel
            {
                ID = entity.ID,
                Slug = entity.Slug,
                Title = entity.Title,
                Excerpt = entity.Excerpt,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate,
                PublishedAt = entity.PublishedAt
            };
        }
    }
}

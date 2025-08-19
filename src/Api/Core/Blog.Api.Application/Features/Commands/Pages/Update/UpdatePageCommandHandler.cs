using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.RequestModels.Pages;
using Blog.Common.Models.Queries.Pages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DomainPage = Blog.Api.Domain.Models.Page;

namespace Blog.Api.Application.Features.Commands.Pages.Update
{
    public class UpdatePageCommandHandler : IRequestHandler<UpdatePageCommand, PageListItemViewModel>
    {
        private readonly IGenericRepository<DomainPage> _repo;
        public UpdatePageCommandHandler(IGenericRepository<DomainPage> repo)
        { _repo = repo; }

        public async Task<PageListItemViewModel> Handle(UpdatePageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.AsQueryable().FirstOrDefaultAsync(x => x.ID == request.Id && !x.isDeleted, cancellationToken);
            if (entity == null) throw new KeyNotFoundException("Page not found");

            if (!string.IsNullOrWhiteSpace(request.Slug)) entity.Slug = request.Slug.Trim();
            if (!string.IsNullOrWhiteSpace(request.Title)) entity.Title = request.Title.Trim();
            if (request.Excerpt != null) entity.Excerpt = request.Excerpt;
            if (request.Content != null) entity.Content = request.Content;
            if (request.Status != null) entity.Status = request.Status;
            if (request.PublishedAt.HasValue) entity.PublishedAt = request.PublishedAt;
            if (request.IsStandalone.HasValue) entity.IsStandalone = request.IsStandalone.Value;
            if (request.MetaTitle != null) entity.MetaTitle = request.MetaTitle;
            if (request.MetaDescription != null) entity.MetaDescription = request.MetaDescription;

            await _repo.UpdateAsync(entity);

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

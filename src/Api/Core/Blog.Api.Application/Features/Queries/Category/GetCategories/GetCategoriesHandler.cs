using MediatR;
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries.Category;

namespace Blog.Api.Application.Queries.Category.GetCategories;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryViewModel>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryViewModel>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAll();

        return categories.Select(c => new CategoryViewModel
        {
            Id = c.ID,
            Name = c.Name,
            Description = c.Description,
            ContentType = c.ContentType,
            ParentId = c.ParentId,
            ParentName = c.ParentId.HasValue ? categories.FirstOrDefault(p => p.ID == c.ParentId)?.Name : null,
            Color = c.Color,
            Icon = c.Icon,
            IsActive = c.IsActive,
            IsFeatured = c.IsFeatured,
            Order = c.Order,
            PostCount = 0,
            ChildrenCount = categories.Count(child => child.ParentId == c.ID),
            CreatedDate = c.CreatedDate,
            UpdatedDate = c.UpdatedDate
        }).OrderBy(c => c.Order).ToList();
    }
}
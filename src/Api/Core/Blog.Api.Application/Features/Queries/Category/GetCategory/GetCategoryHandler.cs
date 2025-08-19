using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Queries.Category;
using MediatR;

namespace Blog.Api.Application.Queries.Category.GetCategory;

public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, CategoryViewModel>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryViewModel> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        
        if (category == null)
        {
            throw new InvalidOperationException("Category not found");
        }

        var allCategories = await _categoryRepository.GetAll();

        return new CategoryViewModel
        {
            Id = category.ID,
            Name = category.Name,
            Description = category.Description,
            ContentType = category.ContentType,
            ParentId = category.ParentId,
            ParentName = category.ParentId.HasValue ? allCategories.FirstOrDefault(p => p.ID == category.ParentId)?.Name : null,
            Color = category.Color,
            Icon = category.Icon,
            IsActive = category.IsActive,
            IsFeatured = category.IsFeatured,
            Order = category.Order,
            PostCount = 0,
            ChildrenCount = allCategories.Count(c => c.ParentId == category.ID),
            CreatedDate = category.CreatedDate
        };
    }
}
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Models.Event.Category;
using MediatR;

namespace Blog.Api.Application.Commands.Category.UpdateCategory;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, UpdateCategoryModel>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<UpdateCategoryModel> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category == null) throw new InvalidOperationException("Category not found");

        category.Name = request.Name;
        category.Description = request.Description;
        category.ContentType = request.ContentType;
        category.ParentId = request.ParentId;
        category.Color = request.Color;
        category.Icon = request.Icon;
        category.IsActive = request.IsActive;
        category.IsFeatured = request.IsFeatured;
        category.Order = request.Order;
        category.UpdatedDate = DateTime.UtcNow;

        await _categoryRepository.UpdateAsync(category);
        return new UpdateCategoryModel 
        { 
            Id = category.ID,
            Name = category.Name,
            Description = category.Description,
            UpdatedDate = category.UpdatedDate ?? DateTime.UtcNow
        };
    }
}
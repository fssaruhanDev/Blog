using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Common.Helpers;
using Blog.Common.Models.Event.Category;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
    if (category == null) throw new Blog.Common.Infrastructure.Exeptions.NotFoundException("Category not found");

        // Parent validation
        if (request.ParentId.HasValue && request.ParentId != category.ID)
        {
            var parent = await _categoryRepository.GetByIdAsync(request.ParentId.Value);
            if (parent == null) throw new Blog.Common.Infrastructure.Exeptions.NotFoundException("Parent category not found");
        }

        // If name changed, update slug and ensure uniqueness
        if (!string.Equals(category.Name, request.Name, StringComparison.Ordinal))
        {
            var newSlug = SlugHelper.GenerateSlug(request.Name);
            if (await _categoryRepository.Get(c => c.Slug == newSlug && c.ID != category.ID).AnyAsync())
                throw new Blog.Common.Infrastructure.Exeptions.BadRequestException("Another category with the same slug exists");
            category.Slug = newSlug;
        }

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

    // slug generation moved to Blog.Common.Helpers.SlugHelper
}
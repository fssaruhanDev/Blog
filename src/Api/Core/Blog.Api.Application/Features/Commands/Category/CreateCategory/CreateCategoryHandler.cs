
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;
using Blog.Common.Models.Event.Category;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blog.Common.Helpers;

namespace Blog.Api.Application.Commands.Category.CreateCategory;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryModel>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CreateCategoryModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // Parent validation
        if (request.ParentId.HasValue)
        {
            var parent = await _categoryRepository.GetByIdAsync(request.ParentId.Value);
            if (parent == null) throw new Blog.Common.Infrastructure.Exeptions.NotFoundException("Parent category not found");
        }

    // Slug generation & uniqueness check
    var slug = SlugHelper.GenerateSlug(request.Name);
        if (await _categoryRepository.Get(c => c.Slug == slug).AnyAsync())
            throw new Blog.Common.Infrastructure.Exeptions.BadRequestException("Category with same slug already exists");

        var category = new Blog.Api.Domain.Models.Category
        {
            ID = Blog.Common.Helpers.UlidHelper.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            ContentType = request.ContentType,
            ParentId = request.ParentId,
            Color = request.Color,
            Icon = request.Icon,
            IsActive = request.IsActive,
            IsFeatured = request.IsFeatured,
            Order = request.Order,
            Slug = slug,
            CreatedDate = DateTime.UtcNow
        };

        await _categoryRepository.AddAsync(category);
        return new CreateCategoryModel 
        { 
            Id = category.ID,
            Name = category.Name,
            Description = category.Description,
            CreatedDate = category.CreatedDate
        };
    }

    // slug generation moved to Blog.Common.Helpers.SlugHelper
}
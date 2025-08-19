
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;
using Blog.Common.Models.Event.Category;
using MediatR;

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
        var category = new Blog.Api.Domain.Models.Category
        {
            ID = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            ContentType = request.ContentType,
            ParentId = request.ParentId,
            Color = request.Color,
            Icon = request.Icon,
            IsActive = request.IsActive,
            IsFeatured = request.IsFeatured,
            Order = request.Order,
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
}
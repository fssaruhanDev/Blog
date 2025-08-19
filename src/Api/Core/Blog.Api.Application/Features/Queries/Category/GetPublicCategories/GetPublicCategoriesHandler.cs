using Blog.Api.Domain.Interfaces.Repositories;
using MediatR;

namespace Blog.Api.Application.Queries.Category.GetPublicCategories;

public class GetPublicCategoriesHandler : IRequestHandler<GetPublicCategoriesQuery, List<GetPublicCategoriesResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetPublicCategoriesHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<GetPublicCategoriesResponse>> Handle(GetPublicCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAll();

        // Only return active categories for public access
        categories = categories.Where(c => c.IsActive).ToList();

        // Apply content type filter if provided
        if (!string.IsNullOrEmpty(request.ContentType))
        {
            categories = categories.Where(c => c.ContentType == request.ContentType).ToList();
        }

        return categories
            .OrderBy(c => c.Order)
            .ThenBy(c => c.Name)
            .Select(c => new GetPublicCategoriesResponse
            {
                Id = c.ID,
                Name = c.Name,
                Description = c.Description,
                ContentType = c.ContentType,
                Color = c.Color,
                Icon = c.Icon,
                Order = c.Order,
                PostCount = 0 // TODO: Implement actual count
            }).ToList();
    }
}
using Blog.Api.Domain.Models;

namespace Blog.Api.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Category>> GetCategoriesByTypeAsync(string contentType);
        Task<List<Category>> GetActiveCategoriesAsync();
        Task<List<Category>> GetFeaturedCategoriesAsync();
        Task<Category?> GetCategoryBySlugAsync(string slug);
        Task<bool> IsSlugUniqueAsync(string slug, Guid? excludeId = null);
        Task<List<Category>> GetChildCategoriesAsync(Guid parentId);
        Task<List<Category>> GetRootCategoriesAsync();
    }
}
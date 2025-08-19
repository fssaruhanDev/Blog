using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;
using Blog.Infrastructure.Persistence.Context;
using Blog.Infrastructure.Persistence.Repostory;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(EntityContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Category>> GetCategoriesByTypeAsync(string contentType)
        {
            return await Get(c => !c.isDeleted)
                .OrderBy(c => c.Order)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<List<Category>> GetActiveCategoriesAsync()
        {
            return await Get(c => !c.isDeleted)
                .OrderBy(c => c.Order)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<List<Category>> GetFeaturedCategoriesAsync()
        {
            return await Get(c => !c.isDeleted)
                .OrderBy(c => c.Order)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryBySlugAsync(string slug)
        {
            return await GetSingleAsync(c => c.Slug == slug);
        }

        public async Task<bool> IsSlugUniqueAsync(string slug, Guid? excludeId = null)
        {
            var query = AsQueryable().Where(c => c.Slug == slug);
            
            if (excludeId.HasValue)
                query = query.Where(c => c.ID != excludeId.Value);
                
            return !await query.AnyAsync();
        }

        public async Task<List<Category>> GetChildCategoriesAsync(Guid parentId)
        {
            return await Get(c => c.ParentId == parentId && !c.isDeleted)
                .OrderBy(c => c.Order)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<List<Category>> GetRootCategoriesAsync()
        {
            return await Get(c => c.ParentId == null && !c.isDeleted)
                .OrderBy(c => c.Order)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }
    }
}
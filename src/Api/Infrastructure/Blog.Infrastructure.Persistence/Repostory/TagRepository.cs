using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;
using Blog.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Repostory;

public class TagRepository : GenericRepository<Tag>, ITagRepository
{
    public TagRepository(EntityContext context) : base(context)
    {
    }

    public async Task<List<Tag>> GetAllTagsAsync(int limit = 100)
    {
        return await entity.Take(limit).OrderBy(t => t.Name).ToListAsync();
    }

    public async Task<List<Tag>> GetPopularTagsAsync(int count = 20)
    {
        return await entity.Where(t => t.IsActive).OrderByDescending(t => t.CreatedDate).Take(count).ToListAsync();
    }

    public async Task<List<Tag>> GetTagsByContentTypeAsync(string contentType)
    {
        return await entity.Where(t => t.ContentType == contentType && t.IsActive).OrderBy(t => t.Name).ToListAsync();
    }

    public async Task<List<Tag>> GetActiveTagsAsync()
    {
        return await entity.Where(t => t.IsActive).OrderBy(t => t.Name).ToListAsync();
    }

    public async Task<List<Tag>> GetFeaturedTagsAsync()
    {
        return await entity.Where(t => t.IsFeatured && t.IsActive).OrderBy(t => t.Name).ToListAsync();
    }
}
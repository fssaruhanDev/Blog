using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;
using Blog.Infrastructure.Persistence.Context;
using Blog.Infrastructure.Persistence.Repostory;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Repository
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(EntityContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Post>> GetPublishedPostsAsync(int page = 1, int pageSize = 10)
        {
            return await Get(p => p.Status == "published" && p.PublishedAt <= DateTime.UtcNow)
                .OrderByDescending(p => p.PublishedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Post>> GetPostsByAuthorAsync(Guid authorId, int page = 1, int pageSize = 10)
        {
            return await Get(p => p.AuthorId == authorId)
                .OrderByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Post>> GetFeaturedPostsAsync(int count = 5)
        {
            return await Get(p => p.IsFeatured && p.Status == "published" && p.PublishedAt <= DateTime.UtcNow)
                .OrderByDescending(p => p.PublishedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Post?> GetPostByTitleAsync(string title)
        {
            return await GetSingleAsync(p => p.Title == title);
        }

        public async Task<bool> IsTitleUniqueAsync(string title, Guid? excludeId = null)
        {
            var query = AsQueryable().Where(p => p.Title == title);
            
            if (excludeId.HasValue)
                query = query.Where(p => p.ID != excludeId.Value);
                
            return !await query.AnyAsync();
        }

        public async Task<List<Post>> SearchPostsAsync(string searchTerm, int page = 1, int pageSize = 10)
        {
            return await Get(p => p.Status == "published" && 
                                 p.PublishedAt <= DateTime.UtcNow &&
                                 (p.Title.Contains(searchTerm) || 
                                  p.Content.Contains(searchTerm) || 
                                  p.Excerpt.Contains(searchTerm)))
                .OrderByDescending(p => p.PublishedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
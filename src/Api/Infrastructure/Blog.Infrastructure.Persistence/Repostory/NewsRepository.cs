using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;
using Blog.Infrastructure.Persistence.Context;
using Blog.Infrastructure.Persistence.Repostory;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Repository
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        public NewsRepository(EntityContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<News>> GetPublishedNewsAsync(int page = 1, int pageSize = 10)
        {
            return await Get(n => n.Status == "published" && n.PublishedAt <= DateTime.UtcNow)
                .OrderByDescending(n => n.PublishedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<News>> GetNewsByStatusAsync(string status, int page = 1, int pageSize = 10)
        {
            return await Get(n => n.Status == status)
                .OrderByDescending(n => n.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<News>> GetNewsByTagsAsync(string tag, int page = 1, int pageSize = 10)
        {
            return await Get(n => n.Tags != null && n.Tags.Contains(tag) && n.Status == "published")
                .OrderByDescending(n => n.PublishedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<News?> GetNewsByTitleAsync(string title)
        {
            return await GetSingleAsync(n => n.Title == title);
        }

        public async Task<bool> IsTitleUniqueAsync(string title, Guid? excludeId = null)
        {
            var query = AsQueryable().Where(n => n.Title == title);
            
            if (excludeId.HasValue)
                query = query.Where(n => n.ID != excludeId.Value);
                
            return !await query.AnyAsync();
        }

    public async Task<List<News>> SearchNewsAsync(string searchTerm, int page = 1, int pageSize = 10)
    {
        // Ensure null-safe checks for Summary and Tags
        searchTerm = searchTerm ?? string.Empty;
        return await Get(n => n.Status == "published" &&
                 n.PublishedAt <= DateTime.UtcNow &&
                 (n.Title.Contains(searchTerm) ||
                  (n.Summary ?? "").Contains(searchTerm) ||
                  (n.Tags ?? "").Contains(searchTerm)))
        .OrderByDescending(n => n.PublishedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

        public async Task<List<News>> GetRecentNewsAsync(int count = 5)
        {
            return await Get(n => n.Status == "published" && n.PublishedAt <= DateTime.UtcNow)
                .OrderByDescending(n => n.PublishedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<string>> GetAllTagsAsync()
        {
            var allTags = await Get(n => !string.IsNullOrEmpty(n.Tags))
                .Select(n => n.Tags)
                .ToListAsync();

            var tagSet = new HashSet<string>();
            foreach (var tagString in allTags)
            {
                if (!string.IsNullOrEmpty(tagString))
                {
                    var tags = tagString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                       .Select(t => t.Trim())
                                       .Where(t => !string.IsNullOrEmpty(t));
                    foreach (var tag in tags)
                    {
                        tagSet.Add(tag);
                    }
                }
            }

            return tagSet.OrderBy(t => t).ToList();
        }
    }
}
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;
using Blog.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Repostory
{
    public class ProjectTagRepository : GenericRepository<ProjectTag>, IProjectTagRepository
    {
        private readonly EntityContext _context;

        public ProjectTagRepository(EntityContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ProjectTag>> GetActiveTagsAsync()
        {
            return await _context.ProjectTags
                .Where(t => t.IsActive && !t.isDeleted)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<List<ProjectTag>> GetTagsWithProjectCountAsync()
        {
            return await _context.ProjectTags
                .Where(t => t.IsActive && !t.isDeleted)
                .Select(t => new ProjectTag
                {
                    ID = t.ID,
                    Name = t.Name,
                    Slug = t.Slug,
                    Color = t.Color,
                    Description = t.Description,
                    IsActive = t.IsActive,
                    Projects = t.Projects!.Where(p => p.IsPublic && !p.isDeleted).ToList()
                })
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<ProjectTag?> GetTagBySlugAsync(string slug)
        {
            return await _context.ProjectTags
                .Where(t => t.Slug == slug && t.IsActive && !t.isDeleted)
                .Include(t => t.Projects)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsSlugUniqueAsync(string slug, Guid? excludeId = null)
        {
            var query = _context.ProjectTags.Where(t => t.Slug == slug && !t.isDeleted);
            if (excludeId.HasValue)
                query = query.Where(t => t.ID != excludeId.Value);
            
            return !await query.AnyAsync();
        }

        public async Task<List<ProjectTag>> GetPopularTagsAsync(int count = 10)
        {
            return await _context.ProjectTags
                .Where(t => t.IsActive && !t.isDeleted)
                .Include(t => t.Projects)
                .OrderByDescending(t => t.Projects!.Count(p => p.IsPublic && !p.isDeleted))
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<ProjectTag>> GetTagsByProjectAsync(Guid projectId)
        {
            return await _context.ProjectTags
                .Where(t => t.Projects!.Any(p => p.ID == projectId) && t.IsActive && !t.isDeleted)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<List<ProjectTag>> GetAllTagsAsync(int limit = 100)
        {
            return await _context.ProjectTags
                .Where(t => t.IsActive && !t.isDeleted)
                .OrderBy(t => t.Name)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<int> GetProjectCountByTagAsync(Guid tagId)
        {
            return await _context.ProjectTags
                .Where(t => t.ID == tagId && t.IsActive && !t.isDeleted)
                .SelectMany(t => t.Projects!)
                .CountAsync(p => p.IsPublic && !p.isDeleted);
        }
    }
}
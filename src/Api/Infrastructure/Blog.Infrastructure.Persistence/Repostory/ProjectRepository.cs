using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;
using Blog.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Repostory
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        private readonly EntityContext _context;

        public ProjectRepository(EntityContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetPublicProjectsAsync(int page = 1, int pageSize = 10)
        {
            return await _context.Projects
                .Where(p => p.IsPublic && !p.isDeleted)
                .Include(p => p.ProjectCategories)
                .Include(p => p.ProjectTags)
                .OrderByDescending(p => p.IsFeatured)
                .ThenBy(p => p.Order)
                .ThenByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Project>> GetFeaturedProjectsAsync(int count = 6)
        {
            return await _context.Projects
                .Where(p => p.IsPublic && p.IsFeatured && !p.isDeleted)
                .Include(p => p.ProjectCategories)
                .Include(p => p.ProjectTags)
                .OrderBy(p => p.Order)
                .ThenByDescending(p => p.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Project>> GetProjectsByCategoryAsync(Guid categoryId, int page = 1, int pageSize = 10)
        {
            return await _context.Projects
                .Where(p => p.ProjectCategories!.Any(pc => pc.CategoryId == categoryId) && p.IsPublic && !p.isDeleted)
                .Include(p => p.ProjectCategories)
                .Include(p => p.ProjectTags)
                .OrderByDescending(p => p.IsFeatured)
                .ThenBy(p => p.Order)
                .ThenByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Project>> GetProjectsByTagAsync(Guid tagId, int page = 1, int pageSize = 10)
        {
            return await _context.Projects
                .Where(p => p.ProjectTags!.Any(pt => pt.ID == tagId) && p.IsPublic && !p.isDeleted)
                .Include(p => p.ProjectCategories)
                .Include(p => p.ProjectTags)
                .OrderByDescending(p => p.IsFeatured)
                .ThenBy(p => p.Order)
                .ThenByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Project>> GetProjectsByTypeAsync(ProjectType type, int page = 1, int pageSize = 10)
        {
            return await _context.Projects
                .Where(p => p.Type == type && p.IsPublic && !p.isDeleted)
                .Include(p => p.ProjectCategories)
                .Include(p => p.ProjectTags)
                .OrderByDescending(p => p.IsFeatured)
                .ThenBy(p => p.Order)
                .ThenByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Project>> GetProjectsByStatusAsync(ProjectStatus status, int page = 1, int pageSize = 10)
        {
            return await _context.Projects
                .Where(p => p.Status == status && p.IsPublic && !p.isDeleted)
                .Include(p => p.ProjectCategories)
                .Include(p => p.ProjectTags)
                .OrderByDescending(p => p.IsFeatured)
                .ThenBy(p => p.Order)
                .ThenByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Project?> GetProjectBySlugAsync(string slug)
        {
            return await _context.Projects
                .Where(p => p.Slug == slug && p.IsPublic && !p.isDeleted)
                .Include(p => p.ProjectCategories)
                .Include(p => p.ProjectTags)
                .FirstOrDefaultAsync();
        }

        public async Task<Project?> GetProjectWithDetailsAsync(Guid id)
        {
            return await _context.Projects
                .Where(p => p.ID == id && !p.isDeleted)
                .Include(p => p.ProjectCategories)
                .Include(p => p.ProjectTags)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Project>> SearchProjectsAsync(string searchTerm, int page = 1, int pageSize = 10)
        {
            var term = searchTerm.ToLower();
            return await _context.Projects
                .Where(p => p.IsPublic && !p.isDeleted &&
                    (p.Title.ToLower().Contains(term) ||
                     p.Description.ToLower().Contains(term)))
                .Include(p => p.ProjectCategories)
                .Include(p => p.ProjectTags)
                .OrderByDescending(p => p.IsFeatured)
                .ThenBy(p => p.Order)
                .ThenByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> IsSlugUniqueAsync(string slug, Guid? excludeId = null)
        {
            var query = _context.Projects.Where(p => p.Slug == slug && !p.isDeleted);
            if (excludeId.HasValue)
                query = query.Where(p => p.ID != excludeId.Value);
            
            return !await query.AnyAsync();
        }

        public async Task IncrementViewCountAsync(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.ViewCount++;
                await _context.SaveChangesAsync();
            }
        }

        public async Task IncrementLikeCountAsync(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.LikeCount++;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Project>> GetRelatedProjectsAsync(Guid projectId, int count = 4)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectCategories)
                .Include(p => p.ProjectTags)
                .FirstOrDefaultAsync(p => p.ID == projectId);

            if (project == null) return new List<Project>();

            var relatedQuery = _context.Projects
                .Where(p => p.ID != projectId && p.IsPublic && !p.isDeleted);

            // Same categories first
            if (project.ProjectCategories?.Any() == true)
            {
                var categoryIds = project.ProjectCategories.Select(pc => pc.CategoryId).ToList();
                relatedQuery = relatedQuery
                    .OrderBy(p => p.ProjectCategories!.Any(pc => categoryIds.Contains(pc.CategoryId)) ? 0 : 1);
            }

            return await relatedQuery
                .Include(p => p.ProjectCategories)
                .Include(p => p.ProjectTags)
                .OrderByDescending(p => p.IsFeatured)
                .ThenBy(p => p.Order)
                .ThenByDescending(p => p.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetProjectCountByCategoryAsync(Guid categoryId)
        {
            return await _context.Projects
                .Where(p => p.ProjectCategories!.Any(pc => pc.CategoryId == categoryId) && p.IsPublic && !p.isDeleted)
                .CountAsync();
        }

        public async Task<Dictionary<ProjectType, int>> GetProjectCountByTypeAsync()
        {
            return await _context.Projects
                .Where(p => p.IsPublic && !p.isDeleted)
                .GroupBy(p => p.Type)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<ProjectStatus, int>> GetProjectCountByStatusAsync()
        {
            return await _context.Projects
                .Where(p => p.IsPublic && !p.isDeleted)
                .GroupBy(p => p.Status)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }
    }
}
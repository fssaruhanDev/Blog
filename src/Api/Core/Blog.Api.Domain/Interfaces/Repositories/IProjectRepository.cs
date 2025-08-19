using Blog.Api.Domain.Models;

namespace Blog.Api.Domain.Interfaces.Repositories
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<List<Project>> GetPublicProjectsAsync(int page = 1, int pageSize = 10);
        Task<List<Project>> GetFeaturedProjectsAsync(int count = 6);
        Task<List<Project>> GetProjectsByCategoryAsync(Guid categoryId, int page = 1, int pageSize = 10);
        Task<List<Project>> GetProjectsByTagAsync(Guid tagId, int page = 1, int pageSize = 10);
        Task<List<Project>> GetProjectsByTypeAsync(ProjectType type, int page = 1, int pageSize = 10);
        Task<List<Project>> GetProjectsByStatusAsync(ProjectStatus status, int page = 1, int pageSize = 10);
        Task<Project?> GetProjectBySlugAsync(string slug);
        Task<Project?> GetProjectWithDetailsAsync(Guid id);
        Task<List<Project>> SearchProjectsAsync(string searchTerm, int page = 1, int pageSize = 10);
        Task<bool> IsSlugUniqueAsync(string slug, Guid? excludeId = null);
        Task IncrementViewCountAsync(Guid id);
        Task IncrementLikeCountAsync(Guid id);
        Task<List<Project>> GetRelatedProjectsAsync(Guid projectId, int count = 4);
        Task<int> GetProjectCountByCategoryAsync(Guid categoryId);
        Task<Dictionary<ProjectType, int>> GetProjectCountByTypeAsync();
        Task<Dictionary<ProjectStatus, int>> GetProjectCountByStatusAsync();
    }
}
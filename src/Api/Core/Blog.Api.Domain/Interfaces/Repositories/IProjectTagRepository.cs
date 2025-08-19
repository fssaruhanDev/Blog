using Blog.Api.Domain.Models;

namespace Blog.Api.Domain.Interfaces.Repositories
{
    public interface IProjectTagRepository : IGenericRepository<ProjectTag>
    {
        Task<List<ProjectTag>> GetActiveTagsAsync();
        Task<List<ProjectTag>> GetAllTagsAsync(int limit = 100);
        Task<List<ProjectTag>> GetTagsWithProjectCountAsync();
        Task<ProjectTag?> GetTagBySlugAsync(string slug);
        Task<bool> IsSlugUniqueAsync(string slug, Guid? excludeId = null);
        Task<List<ProjectTag>> GetPopularTagsAsync(int count = 10);
        Task<List<ProjectTag>> GetTagsByProjectAsync(Guid projectId);
        Task<int> GetProjectCountByTagAsync(Guid tagId);
    }
}
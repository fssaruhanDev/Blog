using Blog.Api.Domain.Models;

namespace Blog.Api.Domain.Interfaces.Repositories
{
    public interface INewsRepository : IGenericRepository<News>
    {
        Task<List<News>> GetPublishedNewsAsync(int page = 1, int pageSize = 10);
        Task<List<News>> GetNewsByStatusAsync(string status, int page = 1, int pageSize = 10);
        Task<List<News>> GetNewsByTagsAsync(string tag, int page = 1, int pageSize = 10);
        Task<News?> GetNewsByTitleAsync(string title);
        Task<bool> IsTitleUniqueAsync(string title, Guid? excludeId = null);
        Task<List<News>> SearchNewsAsync(string searchTerm, int page = 1, int pageSize = 10);
        Task<List<News>> GetRecentNewsAsync(int count = 5);
        Task<List<string>> GetAllTagsAsync();
    }
}
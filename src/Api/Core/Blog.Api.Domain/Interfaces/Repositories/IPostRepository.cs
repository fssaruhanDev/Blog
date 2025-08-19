using Blog.Api.Domain.Models;

namespace Blog.Api.Domain.Interfaces.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetPublishedPostsAsync(int page = 1, int pageSize = 10);
        Task<List<Post>> GetPostsByAuthorAsync(Guid authorId, int page = 1, int pageSize = 10);
        Task<List<Post>> GetFeaturedPostsAsync(int count = 5);
        Task<Post?> GetPostByTitleAsync(string title);
        Task<bool> IsTitleUniqueAsync(string title, Guid? excludeId = null);
        Task<List<Post>> SearchPostsAsync(string searchTerm, int page = 1, int pageSize = 10);
    }
}
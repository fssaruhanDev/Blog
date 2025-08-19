using Blog.Api.Domain.Models;

namespace Blog.Api.Domain.Interfaces.Repositories;

public interface ITagRepository : IGenericRepository<Tag>
{
    Task<List<Tag>> GetAllTagsAsync(int limit = 100);
    Task<List<Tag>> GetPopularTagsAsync(int count = 20);
    Task<List<Tag>> GetTagsByContentTypeAsync(string contentType);
    Task<List<Tag>> GetActiveTagsAsync();
    Task<List<Tag>> GetFeaturedTagsAsync();
}
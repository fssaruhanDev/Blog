using Microsoft.AspNetCore.Mvc;
using Blog.Api.Domain.Interfaces.Repositories;

namespace Blog.Api.WebApi.Controllers
{
    [ApiController]
    [Route("api/project-tags")]
    public class ProjectTagsController : ControllerBase
    {
        private readonly IProjectTagRepository _projectTagRepository;

        public ProjectTagsController(IProjectTagRepository projectTagRepository)
        {
            _projectTagRepository = projectTagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectTags(
            [FromQuery] string? search = null,
            [FromQuery] int limit = 100)
        {
            try
            {
                var tags = await _projectTagRepository.GetAllTagsAsync(limit);
                
                if (!string.IsNullOrEmpty(search))
                {
                    tags = tags.Where(t => t.Name?.Contains(search, StringComparison.OrdinalIgnoreCase) == true).ToList();
                }

                return Ok(tags);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving project tags", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectTag(Guid id)
        {
            try
            {
                var tag = await _projectTagRepository.GetByIdAsync(id);
                
                if (tag == null)
                {
                    return NotFound(new { message = "Project tag not found" });
                }

                return Ok(tag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving project tag", error = ex.Message });
            }
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularTags([FromQuery] int count = 20)
        {
            try
            {
                var tags = await _projectTagRepository.GetPopularTagsAsync(count);
                return Ok(tags);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving popular tags", error = ex.Message });
            }
        }

        [HttpGet("{tagId}/projects")]
        public async Task<IActionResult> GetProjectsByTag(
            Guid tagId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var tag = await _projectTagRepository.GetByIdAsync(tagId);
                if (tag == null)
                {
                    return NotFound(new { message = "Tag not found" });
                }

                // This would need to be implemented in ProjectRepository
                // For now, return empty result
                return Ok(new 
                { 
                    items = new List<object>(), 
                    total = 0, 
                    page, 
                    pageSize,
                    tag = tag.Name
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving projects by tag", error = ex.Message });
            }
        }
    }
}
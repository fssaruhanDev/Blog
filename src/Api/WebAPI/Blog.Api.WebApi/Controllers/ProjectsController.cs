using Microsoft.AspNetCore.Mvc;
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;

namespace Blog.Api.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectTagRepository _projectTagRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProjectsController(
            IProjectRepository projectRepository, 
            IProjectTagRepository projectTagRepository,
            ICategoryRepository categoryRepository)
        {
            _projectRepository = projectRepository;
            _projectTagRepository = projectTagRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20,
            [FromQuery] string? search = null,
            [FromQuery] Guid? categoryId = null,
            [FromQuery] ProjectType? type = null,
            [FromQuery] ProjectStatus? status = null,
            [FromQuery] bool featured = false)
        {
            try
            {
                List<Project> projects;

                if (featured)
                {
                    projects = await _projectRepository.GetFeaturedProjectsAsync(pageSize);
                }
                else if (categoryId.HasValue)
                {
                    projects = await _projectRepository.GetProjectsByCategoryAsync(categoryId.Value, page, pageSize);
                }
                else if (type.HasValue)
                {
                    projects = await _projectRepository.GetProjectsByTypeAsync(type.Value, page, pageSize);
                }
                else if (status.HasValue)
                {
                    projects = await _projectRepository.GetProjectsByStatusAsync(status.Value, page, pageSize);
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    projects = await _projectRepository.SearchProjectsAsync(search, page, pageSize);
                }
                else
                {
                    projects = await _projectRepository.GetPublicProjectsAsync(page, pageSize);
                }

                var totalCount = projects.Count; // Basit count kullan

                return Ok(new 
                { 
                    items = projects, 
                    total = totalCount, 
                    page, 
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving projects", error = ex.Message });
            }
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedProjects([FromQuery] int count = 6)
        {
            try
            {
                var projects = await _projectRepository.GetFeaturedProjectsAsync(count);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving featured projects", error = ex.Message });
            }
        }

        [HttpGet("{idOrSlug}")]
        public async Task<IActionResult> GetProject(string idOrSlug)
        {
            try
            {
                Project? project = null;

                // Try to parse as Guid first
                if (Guid.TryParse(idOrSlug, out var id))
                {
                    project = await _projectRepository.GetProjectWithDetailsAsync(id);
                }
                else
                {
                    // Otherwise treat as slug
                    project = await _projectRepository.GetProjectBySlugAsync(idOrSlug);
                }

                if (project == null)
                {
                    return NotFound(new { message = "Project not found" });
                }

                // Increment view count
                await _projectRepository.IncrementViewCountAsync(project.ID);

                return Ok(project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving project", error = ex.Message });
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProjectsByCategory(
            Guid categoryId, 
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var projects = await _projectRepository.GetProjectsByCategoryAsync(categoryId, page, pageSize);
                var totalCount = await _projectRepository.GetProjectCountByCategoryAsync(categoryId);

                return Ok(new 
                { 
                    items = projects, 
                    total = totalCount, 
                    page, 
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving projects by category", error = ex.Message });
            }
        }

        [HttpGet("tag/{tagId}")]
        public async Task<IActionResult> GetProjectsByTag(
            Guid tagId, 
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var projects = await _projectRepository.GetProjectsByTagAsync(tagId, page, pageSize);
                var totalCount = await _projectTagRepository.GetProjectCountByTagAsync(tagId);

                return Ok(new 
                { 
                    items = projects, 
                    total = totalCount, 
                    page, 
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving projects by tag", error = ex.Message });
            }
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetProjectsByType(
            ProjectType type, 
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var projects = await _projectRepository.GetProjectsByTypeAsync(type, page, pageSize);
                var totalCount = projects.Count; // Basit count kullan

                return Ok(new 
                { 
                    items = projects, 
                    total = totalCount, 
                    page, 
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving projects by type", error = ex.Message });
            }
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetProjectsByStatus(
            ProjectStatus status, 
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var projects = await _projectRepository.GetProjectsByStatusAsync(status, page, pageSize);
                var totalCount = projects.Count; // Basit count kullan

                return Ok(new 
                { 
                    items = projects, 
                    total = totalCount, 
                    page, 
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving projects by status", error = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProjects(
            [FromQuery] string q, 
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20)
        {
            try
            {
                if (string.IsNullOrEmpty(q))
                {
                    return BadRequest(new { message = "Search query is required" });
                }

                var projects = await _projectRepository.SearchProjectsAsync(q, page, pageSize);
                var totalCount = projects.Count; // Basit count kullan

                return Ok(new 
                { 
                    items = projects, 
                    total = totalCount, 
                    page, 
                    pageSize,
                    query = q,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error searching projects", error = ex.Message });
            }
        }

        [HttpGet("{projectId}/related")]
        public async Task<IActionResult> GetRelatedProjects(Guid projectId, [FromQuery] int count = 4)
        {
            try
            {
                var projects = await _projectRepository.GetRelatedProjectsAsync(projectId, count);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving related projects", error = ex.Message });
            }
        }
    }
}
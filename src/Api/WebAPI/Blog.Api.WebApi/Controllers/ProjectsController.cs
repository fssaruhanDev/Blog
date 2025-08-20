using Microsoft.AspNetCore.Mvc;
using MediatR;
using Blog.Api.Application.Queries.Project.GetPublicProjects;
using Blog.Api.Application.Queries.Project.GetProjectByIdentifier;
using Blog.Api.Application.Queries.Project.GetRelatedProjects;
using Blog.Common.Models.Queries.Project;

namespace Blog.Api.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
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
            var query = new GetPublicProjectsQuery
            {
                Page = page,
                PageSize = pageSize,
                Search = search,
                CategoryId = categoryId,
                Featured = featured
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedProjects([FromQuery] int count = 6)
        {
            var q = new GetPublicProjectsQuery { Page = 1, PageSize = count, Featured = true };
            var res = await _mediator.Send(q);
            return Ok(res.Items);
        }

        [HttpGet("{idOrSlug}")]
        public async Task<IActionResult> GetProject(string idOrSlug)
        {
            var res = await _mediator.Send(new GetProjectByIdentifierQuery { Identifier = idOrSlug });
            if (res == null) return NotFound(new { message = "Project not found" });

            // Increment view count via a small command to keep side-effect in Application layer
            await _mediator.Send(new Blog.Api.Application.Commands.Project.IncrementViewCount.IncrementViewCountCommand { Id = res.Id });

            return Ok(res);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProjectsByCategory(
            Guid categoryId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var q = new GetPublicProjectsQuery { Page = page, PageSize = pageSize, CategoryId = categoryId };
            var res = await _mediator.Send(q);
            return Ok(res);
        }

        [HttpGet("tag/{tagId}")]
        public async Task<IActionResult> GetProjectsByTag(
            Guid tagId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var q = new GetPublicProjectsQuery { Page = page, PageSize = pageSize, TagId = tagId };
            var res = await _mediator.Send(q);
            return Ok(res);
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetProjectsByType(
            ProjectType type,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var q = new GetPublicProjectsQuery { Page = page, PageSize = pageSize /* Type mapping can be added if required */ };
            var res = await _mediator.Send(q);
            return Ok(res);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetProjectsByStatus(
            ProjectStatus status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var q = new GetPublicProjectsQuery { Page = page, PageSize = pageSize /* Status mapping can be added if required */ };
            var res = await _mediator.Send(q);
            return Ok(res);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProjects(
            [FromQuery] string q,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (string.IsNullOrEmpty(q)) return BadRequest(new { message = "Search query is required" });
            var query = new GetPublicProjectsQuery { Page = page, PageSize = pageSize, Search = q };
            var res = await _mediator.Send(query);
            return Ok(new { items = res.Items, total = res.Total, page = res.Page, pageSize = res.PageSize, query = q, totalPages = (int)Math.Ceiling((double)res.Total / res.PageSize) });
        }

        [HttpGet("{projectId}/related")]
        public async Task<IActionResult> GetRelatedProjects(Guid projectId, [FromQuery] int count = 4)
        {
            var res = await _mediator.Send(new GetRelatedProjectsQuery { ProjectId = projectId, Count = count });
            return Ok(res);
        }
    }
}
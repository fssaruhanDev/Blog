using Blog.Common.Models.Queries.News;
using Blog.Api.Application.Features.Commands.News.CreateNews;
using Blog.Api.Application.Features.Commands.News.UpdateNews;
using Blog.Api.Application.Features.Commands.News.DeleteNews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.WebApi.Controllers.News
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        private static readonly string CacheKeyPrefix = "news:list:";
        public NewsController(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator; _cache = cache;
        }

        // Public list
        [HttpGet]
        public async Task<ActionResult<Blog.Common.Models.Queries.PagedResult<NewsListItemViewModel>>> Get(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? search = null,
            [FromQuery] string? status = null)
        {
            var ck = $"{CacheKeyPrefix}{page}:{pageSize}:{search}:{status}";
            if (_cache.TryGetValue(ck, out Blog.Common.Models.Queries.PagedResult<NewsListItemViewModel>? cached) && cached != null)
                return Ok(cached);

            var result = await _mediator.Send(new Blog.Api.Application.Features.Queries.News.GetNews.GetNewsQuery
            {
                Page = page,
                PageSize = pageSize,
                Search = search,
                Status = status,
                PublicOnly = true
            });

            _cache.Set(ck, result, TimeSpan.FromSeconds(30));
            return Ok(result);
        }

        // Public by id
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsDetailViewModel?>> GetById(Guid id)
        {
            var result = await _mediator.Send(new Blog.Api.Application.Features.Queries.News.GetNewsById.GetNewsByIdQuery
            {
                Id = id,
                PublicOnly = true
            });
            if (result is null) return NotFound();
            return Ok(result);
        }

        // Admin create
    [HttpPost]
    [Authorize] // TODO: Eğer test için anonime açmak isterseniz geçici olarak [AllowAnonymous] ekleyin
        public async Task<ActionResult<NewsDetailViewModel>> Create([FromBody] CreateNewsCommand command)
        {
            try
            {
                var created = await _mediator.Send(command);
                // Invalidate list cache (simple approach: clear all keys via prefix not directly supported; rely on short TTL)
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException dupEx)
            {
                return Conflict(new { message = dupEx.Message });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sql && sql.Number == 2601 || ex.InnerException is SqlException sql2 && sql2.Number == 2627)
            {
                return Conflict(new { message = "Bu SourceUrl zaten eklenmiş." });
            }
        }

        // Admin update
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<NewsDetailViewModel>> Update(Guid id, [FromBody] UpdateNewsCommand command)
        {
            if (id != command.Id) return BadRequest("ID uyuşmuyor");
            try
            {
                var updated = await _mediator.Send(command);
                return Ok(updated);
            }
            catch (InvalidOperationException dupEx)
            {
                return Conflict(new { message = dupEx.Message });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sql && (sql.Number == 2601 || sql.Number == 2627))
            {
                return Conflict(new { message = "Bu SourceUrl zaten eklenmiş." });
            }
        }

        // Admin delete (soft)
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(Guid id)
        {
            var success = await _mediator.Send(new DeleteNewsCommand(id));
            if (!success) return NotFound();
            return NoContent();
        }
    }
}

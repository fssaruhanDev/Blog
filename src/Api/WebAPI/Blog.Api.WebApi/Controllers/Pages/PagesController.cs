using Blog.Common.Models.Queries.Pages;
using Blog.Common.Models.RequestModels.Pages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.WebApi.Controllers.Pages
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PagesController(IMediator mediator) { _mediator = mediator; }

        [HttpGet]
        public async Task<ActionResult<Blog.Common.Models.Queries.PagedResult<PageListItemViewModel>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null)
        {
            var result = await _mediator.Send(new Blog.Api.Application.Features.Queries.Pages.GetPages.GetPagesQuery
            {
                Page = page,
                PageSize = pageSize,
                Search = search,
                PublicOnly = true
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PageListItemViewModel?>> GetById(Guid id)
        {
            var result = await _mediator.Send(new Blog.Api.Application.Features.Queries.Pages.GetPageById.GetPageByIdQuery
            {
                Id = id,
                PublicOnly = true
            });
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PageListItemViewModel>> Create([FromBody] CreatePageCommand command)
        {
            var created = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<PageListItemViewModel>> Update(Guid id, [FromBody] UpdatePageCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");
            var updated = await _mediator.Send(command);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(Guid id)
        {
            var ok = await _mediator.Send(new DeletePageCommand { Id = id });
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}

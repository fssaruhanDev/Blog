using Blog.Api.Application.Commands.Tag.CreateTag;
using Blog.Api.Application.Commands.Tag.UpdateTag;
using Blog.Api.Application.Commands.Tag.DeleteTag;
using Blog.Api.Application.Queries.Tag.GetTags;
using Blog.Api.Application.Queries.Tag.GetTag;
using Blog.Api.Application.Queries.Tag.GetPublicTags;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TagsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPublicTags([FromQuery] string? contentType = null)
    {
        var query = new GetPublicTagsQuery { ContentType = contentType };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTag(Guid id)
    {
        var query = new GetTagQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

[ApiController]
[Route("api/admin/tags")]
[Authorize]
public class AdminTagsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminTagsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetTags([FromQuery] GetTagsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTag(Guid id)
    {
        var query = new GetTagQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTag), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTag(Guid id, [FromBody] UpdateTagCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(Guid id, [FromQuery] bool forceDelete = false)
    {
        var command = new DeleteTagCommand { Id = id, ForceDelete = forceDelete };
        await _mediator.Send(command);
        return NoContent();
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Blog.Api.Application.Commands.Project.CreateProject;
using Blog.Api.Application.Commands.Project.UpdateProject;
using Blog.Api.Application.Commands.Project.DeleteProject;
using Blog.Api.Application.Queries.Project.GetProjects;
using Blog.Api.Application.Queries.Project.GetProjectById;


namespace Blog.Api.WebApi.Controllers;

[ApiController]
[Route("api/admin/projects")]
[Authorize]
public class AdminProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetProjectsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery { Id = id });
        if (result == null) return NotFound(new { message = "Project not found" });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand command)
    {
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectCommand command)
    {
        command.Id = id;
        var updated = await _mediator.Send(command);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _mediator.Send(new DeleteProjectCommand { Id = id });
        if (!success) return NotFound();
        return NoContent();
    }
}

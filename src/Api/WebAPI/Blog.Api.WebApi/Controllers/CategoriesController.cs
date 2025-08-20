using Blog.Api.Application.Commands.Category.CreateCategory;
using Blog.Api.Application.Commands.Category.UpdateCategory;
using Blog.Api.Application.Commands.Category.DeleteCategory;
using Blog.Api.Application.Queries.Category.GetCategories;
using Blog.Api.Application.Queries.Category.GetCategory;
using Blog.Api.Application.Queries.Category.GetPublicCategories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPublicCategories([FromQuery] string? contentType = null)
    {
        var query = new GetPublicCategoriesQuery { ContentType = contentType };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var query = new GetCategoryQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

// Admin controller moved to separate file: AdminCategoriesController.cs
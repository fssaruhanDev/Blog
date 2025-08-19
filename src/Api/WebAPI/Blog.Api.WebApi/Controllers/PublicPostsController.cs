using System.Threading.Tasks;
using Blog.Api.Application.Features.Queries.Post.GetPublicPosts;
using Blog.Api.Application.Features.Queries.Post.GetPublicPostById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.WebAPI.Controllers
{
    [ApiController]
    [Route("api/public/posts")]
    public class PublicPostsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public PublicPostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] GetPublicPostsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetPublicPostByIdQuery { Id = id });
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeatured([FromQuery] int page = 1, [FromQuery] int pageSize = 6)
        {
            var query = new GetPublicPostsQuery 
            { 
                Page = page, 
                PageSize = pageSize, 
                FeaturedOnly = true 
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
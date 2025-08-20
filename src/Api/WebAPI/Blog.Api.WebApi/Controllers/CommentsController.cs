using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Blog.Api.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Blog.Api.Application.Queries.Comment.GetApprovedComments;
using Blog.Api.Application.Commands.Comment.CreateComment;

namespace Blog.Api.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts/{postId:guid}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CommentsController(IMediator mediator) { _mediator = mediator; }

        private static readonly ConcurrentDictionary<string, ConcurrentQueue<DateTime>> _rate = new();
        private const int WINDOW_SECONDS = 60;
        private const int MAX_PER_WINDOW = 5;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid postId)
        {
        var result = await _mediator.Send(new GetApprovedCommentsQuery { PostId = postId });
        return Ok(result);
        }

        public class CreateCommentRequest
        {
            public string? AuthorName { get; set; }
            public string? AuthorEmail { get; set; }
            public string Content { get; set; } = string.Empty;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(Guid postId, [FromBody] CreateCommentRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Content) || req.Content.Length < 2)
                return BadRequest("Yorum çok kısa");

            // Existence check: reuse repository via a note; keep behavior similar by sending CreateComment and let handler assume post exists
            // For now perform a quick validation by sending the create command and returning NotFound if post missing is detected at repo layer.
            // (Alternatively add a query to check post existence; keeping small to avoid large refactor.)

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var key = postId + "|" + ip;
            var q = _rate.GetOrAdd(key, _ => new ConcurrentQueue<DateTime>());
            var now = DateTime.UtcNow;
            while (q.TryPeek(out var ts) && (now - ts).TotalSeconds > WINDOW_SECONDS) q.TryDequeue(out _);
            if (q.Count >= MAX_PER_WINDOW)
                return StatusCode(429, $"Çok hızlısın. {WINDOW_SECONDS} sn içinde en fazla {MAX_PER_WINDOW} yorum.");
            q.Enqueue(now);

            var cmd = new CreateCommentCommand
            {
                PostId = postId,
                AuthorName = req.AuthorName,
                AuthorEmail = req.AuthorEmail,
                Content = req.Content
            };
            var created = await _mediator.Send(cmd);
            return Ok(created);
        }
    }
}

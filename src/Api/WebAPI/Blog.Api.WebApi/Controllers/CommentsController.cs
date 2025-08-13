using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Blog.Api.Domain.Models;
using Blog.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts/{postId:guid}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly EntityContext _ctx;
        public CommentsController(EntityContext ctx) { _ctx = ctx; }

        private static readonly ConcurrentDictionary<string, ConcurrentQueue<DateTime>> _rate = new();
        private const int WINDOW_SECONDS = 60;
        private const int MAX_PER_WINDOW = 5;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid postId)
        {
            var comments = await _ctx.Comments
                .Where(c => c.PostId == postId && c.Status == "approved")
                .OrderBy(c => c.CreatedDate)
                .Select(c => new { c.ID, c.Content, c.AuthorName, c.CreatedDate })
                .ToListAsync();
            return Ok(comments);
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

            var postExists = await _ctx.Posts.AnyAsync(p => p.ID == postId);
            if (!postExists) return NotFound("Post yok");

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var key = postId + "|" + ip;
            var q = _rate.GetOrAdd(key, _ => new ConcurrentQueue<DateTime>());
            var now = DateTime.UtcNow;
            while (q.TryPeek(out var ts) && (now - ts).TotalSeconds > WINDOW_SECONDS) q.TryDequeue(out _);
            if (q.Count >= MAX_PER_WINDOW)
                return StatusCode(429, $"Çok hızlısın. {WINDOW_SECONDS} sn içinde en fazla {MAX_PER_WINDOW} yorum.");
            q.Enqueue(now);

            var comment = new Comment
            {
                ID = Guid.NewGuid(),
                PostId = postId,
                Content = req.Content.Trim(),
                AuthorName = (req.AuthorName ?? "Anonim").Trim(),
                AuthorEmail = req.AuthorEmail,
                Status = "approved"
            };
            _ctx.Comments.Add(comment);
            await _ctx.SaveChangesAsync();
            return Ok(new { comment.ID, comment.Content, comment.AuthorName, comment.CreatedDate });
        }
    }
}

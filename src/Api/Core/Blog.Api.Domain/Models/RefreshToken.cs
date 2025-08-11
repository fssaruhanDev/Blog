using System;

namespace Blog.Api.Domain.Models
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? UserAgent { get; set; }
        public string? Ip { get; set; }
    }
}

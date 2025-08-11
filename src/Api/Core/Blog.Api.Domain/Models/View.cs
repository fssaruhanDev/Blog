using System;

namespace Blog.Api.Domain.Models
{
    public class View : BaseEntity
    {
        public Guid PostId { get; set; }
        public Guid? UserId { get; set; }
        public string? SessionId { get; set; }
        public string? Ip { get; set; }
        public string? UserAgent { get; set; }
    }
}

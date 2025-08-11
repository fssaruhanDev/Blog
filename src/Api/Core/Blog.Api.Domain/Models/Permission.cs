using System;

namespace Blog.Api.Domain.Models
{
    public class Permission : BaseEntity
    {
        public string Key { get; set; } = default!;
        public string? Description { get; set; }
    }
}

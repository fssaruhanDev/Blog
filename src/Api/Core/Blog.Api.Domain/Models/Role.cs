using System;

namespace Blog.Api.Domain.Models
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}

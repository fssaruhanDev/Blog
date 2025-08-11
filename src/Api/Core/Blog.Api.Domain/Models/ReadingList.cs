using System;

namespace Blog.Api.Domain.Models
{
    public class ReadingList : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = default!;
    }
}

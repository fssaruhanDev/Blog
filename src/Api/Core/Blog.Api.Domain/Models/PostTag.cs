using System;

namespace Blog.Api.Domain.Models
{
    public class PostTag
    {
        public Guid PostId { get; set; }
        public Guid TagId { get; set; }
    }
}

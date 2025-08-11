using System;

namespace Blog.Api.Domain.Models
{
    public class PostCategory
    {
        public Guid PostId { get; set; }
        public Guid CategoryId { get; set; }
    }
}

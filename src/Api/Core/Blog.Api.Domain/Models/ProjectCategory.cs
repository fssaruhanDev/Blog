using System;

namespace Blog.Api.Domain.Models
{
    public class ProjectCategory
    {
        public Guid ProjectId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
using System;

namespace Blog.Api.Domain.Models
{
    public class Redirect : BaseEntity
    {
        public string SourcePath { get; set; } = default!;
        public string TargetUrl { get; set; } = default!;
        public int HttpCode { get; set; } = 301; // 301|302
        public bool IsActive { get; set; } = true;
    }
}

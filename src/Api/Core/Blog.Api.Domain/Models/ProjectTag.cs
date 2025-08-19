using Blog.Api.Domain.Models;

namespace Blog.Api.Domain.Models
{
    public class ProjectTag : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Color { get; set; } // Hex color
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Many-to-many relation
        public List<Project>? Projects { get; set; }
    }
}
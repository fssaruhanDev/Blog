using System;
using System.ComponentModel.DataAnnotations;
using Blog.Common.Models.Queries.Category;
using MediatR;

namespace Blog.Common.Models.RequestModels.Category
{
    public class CreateCategoryCommand : IRequest<CategoryListItemViewModel>
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = default!;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public Guid? ParentId { get; set; }
        
        [Required, MaxLength(50)]
        public string ContentType { get; set; } = default!; // "news", "blog", "project"
        
        [MaxLength(7)]
        public string? Color { get; set; }
        
        [MaxLength(100)]
        public string? Icon { get; set; }
        
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        public int Order { get; set; } = 0;
    }
}
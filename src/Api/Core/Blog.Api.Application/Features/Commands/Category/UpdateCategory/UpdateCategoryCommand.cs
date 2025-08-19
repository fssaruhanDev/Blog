using MediatR;
using Blog.Common.Models.Event.Category;

namespace Blog.Api.Application.Commands.Category.UpdateCategory;

public class UpdateCategoryCommand : IRequest<UpdateCategoryModel>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ContentType { get; set; } = "news";
    public Guid? ParentId { get; set; }
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; } = false;
    public int Order { get; set; } = 0;
}
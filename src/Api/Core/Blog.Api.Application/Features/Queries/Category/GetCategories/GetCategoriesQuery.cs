using MediatR;
using Blog.Common.Models.Queries.Category;

namespace Blog.Api.Application.Queries.Category.GetCategories;

public class GetCategoriesQuery : IRequest<List<CategoryViewModel>>
{
    public string? ContentType { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsFeatured { get; set; }
    public bool IncludeHierarchy { get; set; } = false;
    public string? Search { get; set; }
    public string? OrderBy { get; set; }
    public bool Ascending { get; set; } = true;
}
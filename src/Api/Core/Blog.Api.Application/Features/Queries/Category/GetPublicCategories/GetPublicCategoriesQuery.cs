using MediatR;

namespace Blog.Api.Application.Queries.Category.GetPublicCategories;

public class GetPublicCategoriesQuery : IRequest<List<GetPublicCategoriesResponse>>
{
    public string? ContentType { get; set; }
}
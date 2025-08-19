using MediatR;
using Blog.Common.Models.Queries.Category;

namespace Blog.Api.Application.Queries.Category.GetCategory;

public class GetCategoryQuery : IRequest<CategoryViewModel>
{
    public Guid Id { get; set; }
}
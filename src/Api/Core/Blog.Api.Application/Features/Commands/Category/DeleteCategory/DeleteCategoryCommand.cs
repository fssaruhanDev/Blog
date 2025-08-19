using MediatR;

namespace Blog.Api.Application.Commands.Category.DeleteCategory;

public class DeleteCategoryCommand : IRequest
{
    public Guid Id { get; set; }
    public bool ForceDelete { get; set; } = false;
}
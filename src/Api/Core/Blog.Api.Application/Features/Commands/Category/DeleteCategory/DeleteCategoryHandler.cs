using Blog.Api.Domain.Interfaces.Repositories;
using MediatR;

namespace Blog.Api.Application.Commands.Category.DeleteCategory;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        
        if (category == null)
        {
            throw new InvalidOperationException("Category not found");
        }

        await _categoryRepository.DeleteAsync(category);
        return Unit.Value;
    }
}
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
            throw new Blog.Common.Infrastructure.Exeptions.NotFoundException("Category not found");
        }

        // Check for children categories
        var allCategories = await _categoryRepository.GetAll();
        var children = allCategories.Where(c => c.ParentId == category.ID).ToList();

        if (children.Any() && !request.ForceDelete)
        {
            throw new Blog.Common.Infrastructure.Exeptions.BadRequestException("Category has child categories. Use forceDelete to remove recursively.");
        }

        if (request.ForceDelete)
        {
            // Find all descendants and delete them first
            var toDelete = new List<Guid> { category.ID };
            var queue = new Queue<Guid>();
            queue.Enqueue(category.ID);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var directChildren = allCategories.Where(c => c.ParentId == current).Select(c => c.ID).ToList();
                foreach (var childId in directChildren)
                {
                    toDelete.Add(childId);
                    queue.Enqueue(childId);
                }
            }

            await _categoryRepository.BulkDeleteById(toDelete);
            return Unit.Value;
        }

        await _categoryRepository.DeleteAsync(category);
        return Unit.Value;
    }
}
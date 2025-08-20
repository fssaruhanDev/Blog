using FluentValidation;

namespace Blog.Api.Application.Commands.Category.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(200).WithMessage("Category name must be at most 200 characters.");

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("ContentType is required.")
            .MaximumLength(50);

        RuleFor(x => x.ParentId)
            .Must(id => id == null || id != Guid.Empty).WithMessage("ParentId must be a valid GUID when provided.");
    }
}

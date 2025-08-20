using FluentValidation;

namespace Blog.Api.Application.Commands.Category.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required for update.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(200);

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("ContentType is required.")
            .MaximumLength(50);

        RuleFor(x => x.ParentId)
            .Must((cmd, parentId) => parentId == null || parentId != cmd.Id)
            .WithMessage("ParentId cannot be the same as the category Id.");
    }
}

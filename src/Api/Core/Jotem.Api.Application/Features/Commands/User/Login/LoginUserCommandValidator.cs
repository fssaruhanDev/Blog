using System;
using FluentValidation;
using Jotem.Common.Models.RequestModels;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Api.Application.Features.Commands.User;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
	public LoginUserCommandValidator()
	{

        RuleFor(i => i.UserName)
            .NotNull()
            .WithMessage("(PropertyName} not a valid email address");

        RuleFor(i => i.Password)
             .NotNull()
            .MinimumLength(6).WithMessage("{PropertyName} should at least be (MinLenght} characters");
    }
}


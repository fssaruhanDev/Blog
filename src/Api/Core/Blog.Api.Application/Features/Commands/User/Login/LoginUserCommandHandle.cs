
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Blog.Api.Application.Interfaces.infractucture.Security;
using Blog.Api.Application.Interfaces.Infrastructure.Utility.Logger;
using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Common.Infrastructure;
using Blog.Common.Models.Queries;
using Blog.Common.Models.RequestModels;
using Blog.Infrastructure.Persistence.Exeptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Api.Application.Features.Commands.User;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
{

    private readonly IJwtProvider _jwtProvider;
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private readonly ILoggerService loggerService;


    public LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper, IJwtProvider jwtProvider, ILoggerService loggerService)
    {
        this.mapper = mapper;
        this.userRepository = userRepository;
        this._jwtProvider = jwtProvider;
        this.loggerService = loggerService;
    }

    public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var logProps = new Dictionary<string, object>
        {
            ["UserName"] = request.UserName
        };

        loggerService.LogInformation("Processing LoginUserCommand.", logProps);

        var dbUser = await userRepository.GetSingleAsync(i => i.UserName == request.UserName);
        if (dbUser == null)
        {
            loggerService.LogWarning("Login failed: user not found.", logProps);
            throw new DatabaseValidationException("User not found!");
        }

        var pass = PasswordEncryptor.Encrypt(request.Password);
        if (dbUser.Password != pass)
        {
            loggerService.LogWarning("Login failed: password mismatch.", logProps);
            throw new DatabaseValidationException("Password is wrong!");
        }

        if (!dbUser.EmailConfirmed)
        {
            loggerService.LogWarning("Login failed: email not confirmed.", logProps);
            throw new DatabaseValidationException("Email address is not confirm yet!");
        }

        logProps["UserId"] = dbUser.ID;

        var result = mapper.Map<LoginUserViewModel>(dbUser);

        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, dbUser.ID.ToString()),
            new(ClaimTypes.Email, dbUser.EmailAddress),
            new(ClaimTypes.Name, dbUser.UserName),
            new(ClaimTypes.GivenName, dbUser.FirstName),
            new(ClaimTypes.Surname, dbUser.LastName)
        };

        var expDate = DateTime.Now.AddDays(10);
        result.Token = _jwtProvider.GenerateToken(claims, expDate);

        loggerService.LogInformation("User logged in successfully.", logProps);

        return result;

    }

  
}


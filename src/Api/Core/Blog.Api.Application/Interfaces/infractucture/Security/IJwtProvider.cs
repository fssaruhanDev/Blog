using System.Security.Claims;

namespace Blog.Api.Application.Interfaces.infractucture.Security;

public interface IJwtProvider
{
    string GenerateToken(Claim[] claims, DateTime expiresAt);
}

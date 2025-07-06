using System.Security.Claims;

namespace Jotem.Api.Application.Interfaces.infractucture.Security;

public interface IJwtProvider
{
    string GenerateToken(Claim[] claims, DateTime expiresAt);
}

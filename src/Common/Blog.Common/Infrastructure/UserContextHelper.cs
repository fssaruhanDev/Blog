using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Blog.Common.Infrastructure
{
    public static class UserContextHelper
    {
        public static bool TryGetUserId(IHttpContextAccessor accessor, out Guid userId)
        {
            userId = Guid.Empty;
            var ctx = accessor?.HttpContext;
            if (ctx == null) return false;

            var candidate = ctx.Items["UserId"] as string;
            var user = ctx.User;
            candidate ??= user?.FindFirst("nameid")?.Value
                      ?? user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? user?.FindFirst("sub")?.Value
                      ?? user?.FindFirst("userid")?.Value;

            return !string.IsNullOrWhiteSpace(candidate) && Guid.TryParse(candidate, out userId);
        }
    }
}

using System;

namespace Blog.Api.Domain.Models
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}

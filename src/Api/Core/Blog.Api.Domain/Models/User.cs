using System;
namespace Blog.Api.Domain.Models
{
	public class User : BaseEntity
	{
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
        public string? Avatar { get; set; }

        public required string EmailAddress { get; set; }
		public bool EmailConfirmed { get; set; }

		public required string UserName { get; set; }
		public required string Password { get; set; }

    }
}


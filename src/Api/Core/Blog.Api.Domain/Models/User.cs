using System;
namespace Blog.Api.Domain.Models
{
	public class User : BaseEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
        public string? Avatar { get; set; }

        public string EmailAddress { get; set; }
		public bool EmailConfirmed { get; set; }

		public string UserName { get; set; }
		public string Password { get; set; }

    }
}


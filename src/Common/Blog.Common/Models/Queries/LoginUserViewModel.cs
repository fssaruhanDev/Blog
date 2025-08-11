using System;
namespace Blog.Common.Models.Queries;

public class LoginUserViewModel
{
	public Guid Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string UserName { get; set; } = string.Empty;
	public string Token { get; set; } = string.Empty;


}


using System;
using Jotem.Common.Models.Queries;
using MediatR;

namespace Jotem.Common.Models.RequestModels;

public class LoginUserCommand : IRequest<LoginUserViewModel>
{

	public string UserName { get;  set; }
	public string Password { get;  set; }


	public LoginUserCommand(string userName, string password)
	{
		UserName = userName;
		Password = password;
	}

}


using Jotem.Common.Models.Event.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jotem.Common.Models.RequestModels.User
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public UpdateUserCommand(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; set; }
        public string Password { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Models.Event.User
{
    public class UpdateUserModel
    {

        public Guid UserID { get; set; }
        public string Password { get; set; }

    }
}

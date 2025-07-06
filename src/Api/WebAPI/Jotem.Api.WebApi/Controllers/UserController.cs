
using Jotem.Common.Models.RequestModels;
using Jotem.Common.Models.RequestModels.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jotem.Api.WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator madiator;

        public UserController(IMediator mediator)
        {
            this.madiator = mediator;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand loginUserCommand)
        {
            var res = await madiator.Send(loginUserCommand);
            return Ok(res);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand updateUserCommand)
        {
            var res = await madiator.Send(updateUserCommand);
            return Ok(res);
        }
    }
}

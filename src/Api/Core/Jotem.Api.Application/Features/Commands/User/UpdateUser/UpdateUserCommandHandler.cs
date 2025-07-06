using Jotem.Api.Application.Interfaces.Infrastructure.Utility.Logger;
using Jotem.Api.Application.Interfaces.Repostrories;
using Jotem.Common.Infrastructure;
using Jotem.Common.Models.Event.User;
using Jotem.Common.Models.RequestModels.User;
using Jotem.Infrastructure.Persistence.Exeptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jotem.Api.Application.Features.Commands.User.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {


        private readonly IUserRepository _userRepository;
        private readonly ILoggerService loggerService;


        public UpdateUserCommandHandler(IUserRepository userRepository, ILoggerService loggerService)
        {
            _userRepository = userRepository;
            this.loggerService = loggerService;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var logProps = new Dictionary<string, object>
            {
                ["UserName"] = request.UserName
            };
            var dbUser = await _userRepository.GetSingleAsync(i => i.UserName == request.UserName);

            if (dbUser == null)
            {
                loggerService.LogWarning("Update failed: User not found.", logProps);
                throw new DatabaseValidationException("User not found!");
            }

            var pass = PasswordEncryptor.Encrypt(request.Password);

            dbUser.Password = pass;
            await _userRepository.UpdateAsync(dbUser);
            logProps["UserId"] = dbUser.ID;
            loggerService.LogInformation("User update successfully.", logProps);

            return true;
        }
    }
}

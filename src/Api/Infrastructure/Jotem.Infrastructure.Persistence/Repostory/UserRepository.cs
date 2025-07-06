using Jotem.Api.Application.Interfaces.Repostrories;
using Jotem.Api.Domain.Models;
using Jotem.Infrastructure.Persistence.Context;
using Jotem.Infrastructure.Persistence.Repostory;
using System;

namespace Jotem.Infrastructure.Persistence.Repository;

public class UserRepository : GenericRepository<User>,IUserRepository
{
    public UserRepository(EntityContext dbContext) : base(dbContext)
    {
    }


}


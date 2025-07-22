using Blog.Api.Application.Interfaces.Repostrories;
using Blog.Api.Domain.Models;
using Blog.Infrastructure.Persistence.Context;
using Blog.Infrastructure.Persistence.Repostory;
using System;

namespace Blog.Infrastructure.Persistence.Repository;

public class UserRepository : GenericRepository<User>,IUserRepository
{
    public UserRepository(EntityContext dbContext) : base(dbContext)
    {
    }


}


using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Blog.Common.Infrastructure.Exeptions;

namespace Blog.Infrastructure.Persistence.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EntityContext>
    {
        public EntityContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ConnectionString");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ApiException("DesignTimeDbContextFactory: environment variable 'ConnectionStrings__ConnectionString' is not set. " +
                    "Set it (or create a design-time configuration) before running dotnet-ef commands.", 500, "DesignTime Error", "https://example.com/probs/design-time");
            }

            var optionsBuilder = new DbContextOptionsBuilder<EntityContext>();
            optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure();
            });

            return new EntityContext(optionsBuilder.Options);
        }
    }
}

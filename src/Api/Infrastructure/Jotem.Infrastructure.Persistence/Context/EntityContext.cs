using System;
using System.Reflection;
using ECommerce.Infrastructure.Persistence.EntityConfigurations.Interceptors;
using Jotem.Api.Domain.Models;
using Jotem.Infrastructure.Persistence.EntityConfigurations.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Jotem.Infrastructure.Persistence.Context
{
    public class EntityContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "dbo";
        private readonly SavingChangesInterceptor _savingChangesInterceptor;

        public EntityContext()
        {

        }

        public EntityContext(DbContextOptions options) : base(options)
        {
        }

        public EntityContext(DbContextOptions options, SavingChangesInterceptor savingChangesInterceptor) : base(options)
        {
            _savingChangesInterceptor = savingChangesInterceptor;
        }



        public DbSet<User> Users { get; set; }
        public DbSet<AuditLogEntity> AuditLogs { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Server=localhost;Database=Jotem;User=fssaruhan;Password=123456Asd;TrustServerCertificate=True;Pooling=true; ";

                optionsBuilder.UseSqlServer(connectionString, x =>
                {
                    x.EnableRetryOnFailure();
                });


            }
            optionsBuilder.AddInterceptors(new AuditLogInterceptor());
            optionsBuilder.AddInterceptors(_savingChangesInterceptor);

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }



    }
}


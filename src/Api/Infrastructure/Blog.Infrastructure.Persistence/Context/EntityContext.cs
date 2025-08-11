using System;
using System.Reflection;
using ECommerce.Infrastructure.Persistence.EntityConfigurations.Interceptors;
using Blog.Api.Domain.Models;
using Blog.Infrastructure.Persistence.EntityConfigurations.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Blog.Infrastructure.Persistence.Context
{
    public class EntityContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "dbo";
    private readonly SavingChangesInterceptor? _savingChangesInterceptor;

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
    public DbSet<Blog.Api.Domain.Models.Post> Posts { get; set; }
    public DbSet<Blog.Api.Domain.Models.Category> Categories { get; set; }
    public DbSet<Blog.Api.Domain.Models.Tag> Tags { get; set; }
    public DbSet<Blog.Api.Domain.Models.PostCategory> PostCategories { get; set; }
    public DbSet<Blog.Api.Domain.Models.PostTag> PostTags { get; set; }
    public DbSet<Blog.Api.Domain.Models.Comment> Comments { get; set; }
    public DbSet<Blog.Api.Domain.Models.Media> Media { get; set; }
    public DbSet<Blog.Api.Domain.Models.Attachment> Attachments { get; set; }
    public DbSet<Blog.Api.Domain.Models.Reaction> Reactions { get; set; }
    public DbSet<Blog.Api.Domain.Models.SeoMeta> SeoMeta { get; set; }
    public DbSet<Blog.Api.Domain.Models.Slug> Slugs { get; set; }
    public DbSet<Blog.Api.Domain.Models.Redirect> Redirects { get; set; }
    public DbSet<Blog.Api.Domain.Models.Role> Roles { get; set; }
    public DbSet<Blog.Api.Domain.Models.Permission> Permissions { get; set; }
    public DbSet<Blog.Api.Domain.Models.UserRole> UserRoles { get; set; }
    public DbSet<Blog.Api.Domain.Models.RolePermission> RolePermissions { get; set; }
    public DbSet<Blog.Api.Domain.Models.RefreshToken> RefreshTokens { get; set; }
    public DbSet<Blog.Api.Domain.Models.View> Views { get; set; }
    public DbSet<Blog.Api.Domain.Models.PostDailyMetric> PostDailyMetrics { get; set; }
    public DbSet<Blog.Api.Domain.Models.Bookmark> Bookmarks { get; set; }
    public DbSet<Blog.Api.Domain.Models.ReadingList> ReadingLists { get; set; }
    public DbSet<Blog.Api.Domain.Models.ReadingListItem> ReadingListItems { get; set; }
    public DbSet<Blog.Api.Domain.Models.News> News { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Fallback connection string for design-time operations
                var connectionString = "Server=FSSARUHAN;Database=BlogDb;User=fssaruhan;Password=123456Asd;TrustServerCertificate=True;Pooling=true;";

                optionsBuilder.UseSqlServer(connectionString, x =>
                {
                    x.EnableRetryOnFailure();
                });


            }
            optionsBuilder.AddInterceptors(new AuditLogInterceptor());
            if (_savingChangesInterceptor != null)
                optionsBuilder.AddInterceptors(_savingChangesInterceptor);

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }



    }
}


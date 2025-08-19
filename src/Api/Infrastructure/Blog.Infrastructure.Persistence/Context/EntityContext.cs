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
    public DbSet<Post> Posts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PostCategory> PostCategories { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Media> Media { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<SeoMeta> SeoMeta { get; set; }
    public DbSet<Slug> Slugs { get; set; }
    public DbSet<Redirect> Redirects { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<View> Views { get; set; }
    public DbSet<PostDailyMetric> PostDailyMetrics { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<ReadingList> ReadingLists { get; set; }
    public DbSet<ReadingListItem> ReadingListItems { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectCategory> ProjectCategories { get; set; }
    public DbSet<ProjectTag> ProjectTags { get; set; }



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


using System;
using System.Reflection;
using Blog.Api.Domain.Models;
using Blog.Infrastructure.Persistence.EntityConfigurations.Interceptors;
using Microsoft.EntityFrameworkCore;
using Blog.Common.Infrastructure.Exeptions;
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

        public EntityContext(DbContextOptions<EntityContext> options) : base(options)
        {
        }

        public EntityContext(DbContextOptions<EntityContext> options, SavingChangesInterceptor savingChangesInterceptor) : base(options)
        {
            _savingChangesInterceptor = savingChangesInterceptor;
        }



    public DbSet<User> Users { get; set; } = null!;
    public DbSet<AuditLogEntity> AuditLogs { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<PostCategory> PostCategories { get; set; } = null!;
    public DbSet<PostTag> PostTags { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Media> Media { get; set; } = null!;
    public DbSet<Attachment> Attachments { get; set; } = null!;
    public DbSet<Reaction> Reactions { get; set; } = null!;
    public DbSet<SeoMeta> SeoMeta { get; set; } = null!;
    public DbSet<Slug> Slugs { get; set; } = null!;
    public DbSet<Redirect> Redirects { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<RolePermission> RolePermissions { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<View> Views { get; set; } = null!;
    public DbSet<PostDailyMetric> PostDailyMetrics { get; set; } = null!;
    public DbSet<Bookmark> Bookmarks { get; set; } = null!;
    public DbSet<ReadingList> ReadingLists { get; set; } = null!;
    public DbSet<ReadingListItem> ReadingListItems { get; set; } = null!;
    public DbSet<News> News { get; set; } = null!;
    public DbSet<Page> Pages { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectCategory> ProjectCategories { get; set; } = null!;
    public DbSet<ProjectTag> ProjectTags { get; set; } = null!;



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // If options are already configured by AddDbContext in the host, do nothing here.
            if (!optionsBuilder.IsConfigured)
            {
                // Read connection string from environment (use double-underscore for ':')
                var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ConnectionString");

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new ApiException("Database connection string is not configured. " +
                        "Set the environment variable 'ConnectionStrings__ConnectionString' or configure the DbContext via AddDbContext in the host application (e.g., Program.cs / Startup).", 500, "Configuration Error", "https://example.com/probs/configuration");
                }

                optionsBuilder.UseSqlServer(connectionString, x =>
                {
                    x.EnableRetryOnFailure();
                });
            }

            // Prefer registering interceptors through DI in AddDbContext; keep fallback to ensure audit logs in simple scenarios.
            optionsBuilder.AddInterceptors(new AuditLogInterceptor());
            if (_savingChangesInterceptor != null)
                optionsBuilder.AddInterceptors(_savingChangesInterceptor);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
            // Use the persistence assembly (where EntityContext is declared) to discover entity configurations.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityContext).Assembly);
        }



    }
}


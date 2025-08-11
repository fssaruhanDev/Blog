using System;
using System.Globalization;
using Bogus;
using Blog.Api.Domain.Models;
using Blog.Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog.Infrastructure.Persistence.Context;

public class SeedData
{
    private static string Slugify(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;
        var normalized = text
            .ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("_", "-")
            .Replace("--", "-");
        return new string(normalized.Where(ch => char.IsLetterOrDigit(ch) || ch == '-').ToArray());
    }

	private static List<User> GetUsers()
	{
        var result = new Faker<User>(locale:"tr")
            .RuleFor(i => i.ID, i => Guid.NewGuid())
            .RuleFor(i => i.CreatedDate, i => i.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
            .RuleFor(i => i.Avatar, i => i.Internet.Avatar())
            .RuleFor(i => i.FirstName, i => i.Person.FirstName)
            .RuleFor(i => i.LastName, i => i.Person.LastName)
            .RuleFor(i => i.EmailAddress, i => i.Internet.Email())
            .RuleFor(i => i.UserName, i => i.Internet.UserName())
			.RuleFor(i => i.Password, i => PasswordEncryptor.Encrypt( i.Internet.Password()))
			.RuleFor(i => i.EmailConfirmed, i => i.PickRandom(true, false))
            .RuleFor(i => i.isActive, _ => true)
            .RuleFor(i => i.isDeleted, _ => false)
            .RuleFor(i => i.isModified, _ => false)
			.Generate(100);

		return result;
    }


	public async Task SeedAsync(IConfiguration configuration)
	{
        try
        {
            var connectionString = configuration.GetConnectionString("ConnectionString")
                                     ?? configuration["ConnectionStrings:ConnectionString"]
                                     ?? throw new InvalidOperationException("Connection string not found");

            var dbContextBuilder = new DbContextOptionsBuilder();
            dbContextBuilder.UseSqlServer(connectionString, x => x.EnableRetryOnFailure());
            await using var context = new EntityContext(dbContextBuilder.Options);

            // USERS + ADMIN
            if (!await context.Users.AsNoTracking().AnyAsync())
            {
                var admin = new User
                {
                    ID = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = Guid.Empty,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailAddress = "admin@example.com",
                    UserName = "admin",
                    Password = PasswordEncryptor.Encrypt("Admin@123"),
                    EmailConfirmed = true,
                    isActive = true,
                    isDeleted = false,
                    isModified = false
                };

                var users = GetUsers();
                users.Insert(0, admin);
                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }

            // ROLES & PERMISSIONS
            if (!await context.Roles.AsNoTracking().AnyAsync())
            {
                var roleAdmin = new Role { ID = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, CreatedBy = Guid.Empty, Name = "Admin", Description = "Administrators", isActive = true };
                var roleEditor = new Role { ID = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, CreatedBy = Guid.Empty, Name = "Editor", Description = "Editors", isActive = true };
                var roleAuthor = new Role { ID = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, CreatedBy = Guid.Empty, Name = "Author", Description = "Authors", isActive = true };
                await context.Roles.AddRangeAsync(roleAdmin, roleEditor, roleAuthor);
                await context.SaveChangesAsync();

                // minimal permissions
                var perms = new[] { "posts.read", "posts.write", "comments.moderate", "users.manage" }
                    .Select(k => new Permission { ID = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, CreatedBy = Guid.Empty, Key = k, Description = k, isActive = true })
                    .ToList();
                await context.Permissions.AddRangeAsync(perms);
                await context.SaveChangesAsync();

                // link some permissions to admin
                var rp = perms.Select(p => new RolePermission { RoleId = roleAdmin.ID, PermissionId = p.ID });
                await context.RolePermissions.AddRangeAsync(rp);
                await context.SaveChangesAsync();

                // assign admin user to Admin role
                var adminUser = await context.Users.AsNoTracking().FirstAsync(u => u.UserName == "admin");
                await context.UserRoles.AddAsync(new UserRole { UserId = adminUser.ID, RoleId = roleAdmin.ID });
                await context.SaveChangesAsync();
            }

            // TAXONOMY: Categories & Tags
            if (!await context.Categories.AsNoTracking().AnyAsync())
            {
                var faker = new Faker("tr");
                var categories = Enumerable.Range(1, 8)
                    .Select(i => new Category
                    {
                        ID = Guid.NewGuid(),
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = Guid.Empty,
                        Name = faker.Commerce.Categories(1)[0],
                        Slug = "",
                        Description = faker.Lorem.Sentence(),
                        isActive = true
                    }).ToList();
                // ensure unique slugs
                var used = new HashSet<string>();
                foreach (var c in categories)
                {
                    var s = Slugify(c.Name);
                    var t = s; var n = 1;
                    while (!used.Add(t)) { t = $"{s}-{n++}"; }
                    c.Slug = t;
                }
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            if (!await context.Tags.AsNoTracking().AnyAsync())
            {
                var faker = new Faker("tr");
                var tags = Enumerable.Range(1, 20)
                    .Select(i => new Tag
                    {
                        ID = Guid.NewGuid(),
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = Guid.Empty,
                        Name = faker.Hacker.Noun(),
                        Slug = "",
                        Description = faker.Lorem.Sentence(),
                        isActive = true
                    }).ToList();
                var used = new HashSet<string>();
                foreach (var t in tags)
                {
                    var s = Slugify(t.Name);
                    var v = s; var n = 1;
                    while (!used.Add(v)) { v = $"{s}-{n++}"; }
                    t.Slug = v;
                }
                await context.Tags.AddRangeAsync(tags);
                await context.SaveChangesAsync();
            }

            // POSTS
            if (!await context.Posts.AsNoTracking().AnyAsync())
            {
                var faker = new Faker("tr");
                var users = await context.Users.AsNoTracking().ToListAsync();
                var catIds = await context.Categories.AsNoTracking().Select(c => c.ID).ToListAsync();
                var tagIds = await context.Tags.AsNoTracking().Select(t => t.ID).ToListAsync();

                var posts = Enumerable.Range(1, 25).Select(i => new Post
                {
                    ID = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow.AddDays(-faker.Random.Int(0, 60)),
                    CreatedBy = Guid.Empty,
                    AuthorId = faker.PickRandom(users).ID,
                    Title = faker.Lorem.Sentence(5),
                    Excerpt = faker.Lorem.Sentences(2),
                    Content = faker.Lorem.Paragraphs(3, "\n\n"),
                    Status = "published",
                    PublishedAt = DateTime.UtcNow.AddDays(-faker.Random.Int(0, 60)),
                    ReadingTime = faker.Random.Int(3, 10),
                    IsFeatured = faker.Random.Bool(0.2f),
                    isActive = true
                }).ToList();

                await context.Posts.AddRangeAsync(posts);
                await context.SaveChangesAsync();

                // slugs & seo
                var slugs = posts.Select(p => new Slug
                {
                    ID = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = Guid.Empty,
                    TargetType = "post",
                    TargetId = p.ID,
                    SlugText = Slugify(p.Title),
                    IsPrimary = true,
                    isActive = true
                }).ToList();
                await context.Slugs.AddRangeAsync(slugs);

                var metas = posts.Select(p => new SeoMeta
                {
                    ID = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = Guid.Empty,
                    TargetType = "post",
                    TargetId = p.ID,
                    MetaTitle = p.Title,
                    MetaDescription = p.Excerpt,
                    isActive = true
                }).ToList();
                await context.SeoMeta.AddRangeAsync(metas);
                await context.SaveChangesAsync();

                // links
                var rand = new Random();
                var pc = new List<PostCategory>();
                var pt = new List<PostTag>();
                foreach (var p in posts)
                {
                    var pickCats = catIds.OrderBy(_ => rand.Next()).Take(rand.Next(1, Math.Min(3, catIds.Count))).ToList();
                    var pickTags = tagIds.OrderBy(_ => rand.Next()).Take(rand.Next(2, Math.Min(5, tagIds.Count))).ToList();
                    pc.AddRange(pickCats.Select(c => new PostCategory { PostId = p.ID, CategoryId = c }));
                    pt.AddRange(pickTags.Select(t => new PostTag { PostId = p.ID, TagId = t }));
                }
                await context.PostCategories.AddRangeAsync(pc.DistinctBy(x => new { x.PostId, x.CategoryId }));
                await context.PostTags.AddRangeAsync(pt.DistinctBy(x => new { x.PostId, x.TagId }));
                await context.SaveChangesAsync();
            }
           

        }
        catch (CultureNotFoundException ex)
        {
            Console.WriteLine($"Hata: {ex.Message}, Invalid Culture Name: {ex.InvalidCultureName}");
        }
       
    }
}


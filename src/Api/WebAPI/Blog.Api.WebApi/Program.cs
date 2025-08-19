using FluentValidation.AspNetCore;
using Blog.Api.Application.Extensions;
using Blog.Common.Middleware;
using Blog.Common.Middlewares;
using Blog.Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Serilog;
using Blog.Infrastructure.Utilities.Logger.Services;
using Blog.Infrastructure.Utilities.Logger.Extensions;
using Blog.Infrastructure.Utilities.Cache.Extensions;
using Blog.Infrastructure.Security.Extensions;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddControllers();
// FluentValidation: use new registration APIs (see deprecation notice)
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
var jwtSecurityKey = builder.Configuration["Token:SecurityKey"] ?? throw new InvalidOperationException("Token:SecurityKey configuration is missing");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Token:Issuer"],
        ValidAudience = builder.Configuration["Token:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecurityKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// CORS for local development (Vite dev server)
const string CorsPolicy = "FrontendCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            // Allow any localhost/127.0.0.1 origin with any port during development
            .SetIsOriginAllowed(origin =>
            {
                try
                {
                    var uri = new Uri(origin);
                    return (uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) || uri.Host.Equals("127.0.0.1"));
                }
                catch { return false; }
            })
            .AllowCredentials()
    );
});

//Log Settings
#region log Settings

#endregion

// Registrations
builder.Services.AddLoggerRegistration(builder.Configuration);
builder.Services.AddJWTRegistration(builder.Configuration);
builder.Services.AddCacheRegistration(builder.Configuration);
builder.Services.AddInfastructureRegistration(builder.Configuration);
builder.Services.AddApplicationRegistration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CurrentUserMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();


// Enable HTTPS redirection only if HTTPS endpoint is configured
var httpsConfigured = (builder.Configuration["ASPNETCORE_URLS"]?.Contains("https://", StringComparison.OrdinalIgnoreCase) ?? false)
                     || !string.IsNullOrEmpty(builder.Configuration["ASPNETCORE_HTTPS_PORTS"]) 
                     || !string.IsNullOrEmpty(builder.Configuration["ASPNETCORE_HTTPS_PORT"]);

if (httpsConfigured)
    app.UseHttpsRedirection();

app.UseCors(CorsPolicy);

// Serve uploaded files (wwwroot/uploads)
var uploadRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
if (!Directory.Exists(uploadRoot)) Directory.CreateDirectory(uploadRoot);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = ""
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    try
    {
        var ctx = scope.ServiceProvider.GetRequiredService<Blog.Infrastructure.Persistence.Context.EntityContext>();
        await ctx.Database.MigrateAsync();
        var startupLogger = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>().CreateLogger("Startup");
        startupLogger.LogInformation("Database migrated successfully");
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>().CreateLogger("Startup");
        logger.LogError(ex, "Migration failed");
        throw;
    }
}

app.Run();

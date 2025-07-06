using Jotem.Api.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jotem.Infrastructure.Persistence.EntityConfigurations.Interceptors;

public class SavingChangesInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SavingChangesInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var entries = eventData.Context.ChangeTracker.Entries().ToList();
        var userId = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();

        foreach (var item in entries.Where(i => i.State == EntityState.Added || i.State == EntityState.Modified))
        {
            if (item.State == EntityState.Modified)
            {
                if (item.Metadata.FindProperty("UpdatedDate") != null)
                {
                    item.CurrentValues["UpdatedDate"] = DateTime.Now;
                }
                
                if (item.Metadata.FindProperty("isModified") != null)
                {
                    item.CurrentValues["isModified"] = true;
                }
                if (!string.IsNullOrEmpty(userId) && item.Metadata.FindProperty("UpdatedBy") != null)
                {
                    item.CurrentValues["UpdatedBy"] = Guid.Parse( userId);
                }
            }
            else if (item.State == EntityState.Added)
            {
                if (item.Metadata.FindProperty("CreatedDate") != null)
                {
                    item.CurrentValues["CreatedDate"] = DateTime.Now;
                } 
                
                if (item.Metadata.FindProperty("isActive") != null)
                {
                    item.CurrentValues["isActive"] = true;
                } 
                
                if (item.Metadata.FindProperty("isDeleted") != null)
                {
                    item.CurrentValues["isDeleted"] = false;
                }

                if (!string.IsNullOrEmpty(userId) && item.Metadata.FindProperty("CreatedBy") != null)
                {
                    item.CurrentValues["CreatedBy"] = Guid.Parse(userId); ;
                }
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}

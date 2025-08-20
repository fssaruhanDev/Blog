using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Blog.Infrastructure.Persistence.EntityConfigurations.Interceptors;

public class AuditLogInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = context.ChangeTracker.Entries().ToList();

        var auditLogs = entries
                        .Where(i => i.Entity is not AuditLogEntity)
                        .Where(i => i.State == EntityState.Added
                            || i.State == EntityState.Modified
                            || i.State == EntityState.Deleted);

        var auditLogEntities = new List<AuditLogEntity>();
        foreach (var entry in auditLogs)
        {
            var tableName = entry.Metadata.GetTableName() ?? entry.Metadata.Name;
            var log = new AuditLogEntity()
            {
                TableName = tableName,
                Operation = entry.State.ToString(),
                CreatedDate = DateTime.UtcNow,
            };

            auditLogEntities.Add(log);

            if (entry.State == EntityState.Modified)
            {
                var oldValue = entry.OriginalValues.Properties.ToDictionary(p => p.Name,
                                                             p => entry.OriginalValues[p]);

                log.OldValue = JsonSerializer.Serialize(oldValue);

                var newValue = entry.CurrentValues.Properties.ToDictionary(p => p.Name,
                                                             p => entry.CurrentValues[p]);

                log.NewValue = JsonSerializer.Serialize(newValue);

            }
            else if (entry.State == EntityState.Added)
            {
                var newValue = entry.CurrentValues.Properties.ToDictionary(p => p.Name,
                                                             p => entry.CurrentValues[p]);

                log.NewValue = JsonSerializer.Serialize(newValue);

            }
            else
            {
                var oldValue = entry.OriginalValues.Properties.ToDictionary(p => p.Name,
                                                             p => entry.OriginalValues[p]);

                log.OldValue = JsonSerializer.Serialize(oldValue);
            }

        }

        if (context != null)
        {
            context.Set<AuditLogEntity>().AddRange(auditLogEntities);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}

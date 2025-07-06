using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Jotem.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECommerce.Infrastructure.Persistence.EntityConfigurations.Interceptors;

public class AuditLogInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var entries = eventData.Context.ChangeTracker.Entries().ToList();

        var auditLogs = entries
                        .Where(i => i.Entity is not AuditLogEntity)
                        .Where(i => i.State == EntityState.Added
                            || i.State == EntityState.Modified
                            || i.State == EntityState.Deleted);

        var auditLogEntities = new List<AuditLogEntity>();
        foreach (var entry in auditLogs)
        {
            var log = new AuditLogEntity()
            {
                TableName = entry.Metadata.GetTableName(),
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

        eventData.Context.Set<AuditLogEntity>().AddRange(auditLogEntities);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}

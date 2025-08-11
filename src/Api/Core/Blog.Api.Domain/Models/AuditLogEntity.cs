

namespace Blog.Api.Domain.Models;
public class AuditLogEntity : BaseEntity
{
    public string Operation { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string OldValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
}

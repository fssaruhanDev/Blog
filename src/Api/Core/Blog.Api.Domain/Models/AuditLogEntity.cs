

namespace Blog.Api.Domain.Models;
public class AuditLogEntity : BaseEntity
{
    public string Operation { get; set; }
    public string TableName { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
}

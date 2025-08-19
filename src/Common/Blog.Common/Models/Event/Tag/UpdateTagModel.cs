namespace Blog.Common.Models.Event.Tag;

public class UpdateTagModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime UpdatedDate { get; set; }
}
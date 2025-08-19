namespace Blog.Common.Models.Event.Tag;

public class CreateTagModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
}
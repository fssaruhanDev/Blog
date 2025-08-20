namespace Blog.Common.Models.Event.Project
{
    public class CreateProjectModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

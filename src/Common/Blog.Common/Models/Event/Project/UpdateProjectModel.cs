namespace Blog.Common.Models.Event.Project
{
    public class UpdateProjectModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
    }
}

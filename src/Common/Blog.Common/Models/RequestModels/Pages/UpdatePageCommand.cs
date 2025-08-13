using System;
using Blog.Common.Models.Queries.Pages;
using MediatR;

namespace Blog.Common.Models.RequestModels.Pages
{
    public class UpdatePageCommand : IRequest<PageListItemViewModel>
    {
        public Guid Id { get; set; }
        public string? Slug { get; set; }
        public string? Title { get; set; }
        public string? Excerpt { get; set; }
        public string? Content { get; set; }
        public string? Status { get; set; }
        public DateTime? PublishedAt { get; set; }
        public bool? IsStandalone { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}

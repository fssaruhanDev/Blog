using System;
using Blog.Common.Models.Queries.Pages;
using MediatR;

namespace Blog.Common.Models.RequestModels.Pages
{
    public class CreatePageCommand : IRequest<PageListItemViewModel>
    {
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Excerpt { get; set; }
        public string? Content { get; set; }
        public string? Status { get; set; }
        public DateTime? PublishedAt { get; set; }
        public bool IsStandalone { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}

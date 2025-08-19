using MediatR;
using Blog.Common.Models.Queries.Tag;

namespace Blog.Api.Application.Queries.Tag.GetTags;

public class GetTagsQuery : IRequest<List<TagViewModel>>
{
    public string? ContentType { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsFeatured { get; set; }
    public string? Search { get; set; }
    public int? Limit { get; set; }
}
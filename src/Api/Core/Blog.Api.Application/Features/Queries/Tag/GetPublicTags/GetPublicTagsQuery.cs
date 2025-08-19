using MediatR;

namespace Blog.Api.Application.Queries.Tag.GetPublicTags;

public class GetPublicTagsQuery : IRequest<List<GetPublicTagsResponse>>
{
    public string? ContentType { get; set; }
}
using MediatR;
using Blog.Common.Models.Queries.Tag;

namespace Blog.Api.Application.Queries.Tag.GetTag;

public class GetTagQuery : IRequest<TagViewModel>
{
    public Guid Id { get; set; }
}
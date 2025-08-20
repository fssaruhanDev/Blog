using MediatR;
using Blog.Common.Models.Queries;

namespace Blog.Api.Application.Queries.Comment.GetApprovedComments;

public class GetApprovedCommentsQuery : IRequest<List<object>>
{
    public Guid PostId { get; set; }
}

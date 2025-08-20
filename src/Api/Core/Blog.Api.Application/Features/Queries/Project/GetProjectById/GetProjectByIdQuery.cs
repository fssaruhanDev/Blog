using MediatR;
using Blog.Common.Models.Queries.Project;

namespace Blog.Api.Application.Queries.Project.GetProjectById;

public class GetProjectByIdQuery : IRequest<ProjectDetailViewModel?>
{
    public Guid Id { get; set; }
}

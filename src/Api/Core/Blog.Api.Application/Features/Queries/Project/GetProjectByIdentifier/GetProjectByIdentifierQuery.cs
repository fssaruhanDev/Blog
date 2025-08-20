using MediatR;
using Blog.Common.Models.Queries.Project;

namespace Blog.Api.Application.Queries.Project.GetProjectByIdentifier;

public class GetProjectByIdentifierQuery : IRequest<ProjectDetailViewModel?>
{
    public string Identifier { get; set; } = string.Empty; // guid or slug
}

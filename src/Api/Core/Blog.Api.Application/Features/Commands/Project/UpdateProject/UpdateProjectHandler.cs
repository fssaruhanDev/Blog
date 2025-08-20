using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Blog.Common.Models.Event.Project;
using MediatR;
using Blog.Common.Helpers;

namespace Blog.Api.Application.Commands.Project.UpdateProject;

public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, UpdateProjectModel>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTagRepository _tagRepository;

    public UpdateProjectHandler(IProjectRepository projectRepository, IProjectTagRepository tagRepository)
    {
        _projectRepository = projectRepository;
        _tagRepository = tagRepository;
    }

    public async Task<UpdateProjectModel> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id);
        if (project == null) throw new Blog.Common.Infrastructure.Exeptions.NotFoundException("Project not found");

        // update fields
        project.Title = request.Title;
        project.Description = request.Description;
        project.ShortDescription = request.ShortDescription;
        project.Content = request.Content;
        project.FeaturedImage = request.FeaturedImage;
        project.Images = request.Images;
        project.TechnologiesUsed = request.TechnologiesUsed != null ? string.Join(',', request.TechnologiesUsed) : project.TechnologiesUsed;
        project.ProjectUrl = request.ProjectUrl;
        project.GithubUrl = request.GithubUrl;
        project.DemoUrl = request.DemoUrl;
        project.StartDate = request.StartDate;
        project.EndDate = request.EndDate;
        project.Status = (ProjectStatus)request.Status;
        project.Type = (ProjectType)request.Type;
        project.ClientName = request.ClientName;
        project.IsFeatured = request.IsFeatured;
        project.IsPublic = request.IsPublic;
        project.Order = request.Order;
        project.UpdatedDate = DateTime.UtcNow;

        // handle tags
        if (request.TagIds != null)
        {
            var tags = await _tagRepository.Get(t => request.TagIds.Contains(t.ID)).ToListAsync();
            project.ProjectTags = tags;
        }

        if (request.NewTags != null && request.NewTags.Any())
        {
            project.ProjectTags = project.ProjectTags ?? new System.Collections.Generic.List<Blog.Api.Domain.Models.ProjectTag>();
            foreach (var t in request.NewTags)
            {
                var newTag = new Blog.Api.Domain.Models.ProjectTag
                {
                    ID = UlidHelper.NewGuid(),
                    Name = t,
                    Slug = SlugHelper.GenerateSlug(t),
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };
                await _tagRepository.AddAsync(newTag);
                project.ProjectTags.Add(newTag);
            }
        }

        await _projectRepository.UpdateAsync(project);

        return new UpdateProjectModel
        {
            Id = project.ID,
            Title = project.Title,
            UpdatedAt = project.UpdatedDate ?? project.CreatedDate
        };
    }
}

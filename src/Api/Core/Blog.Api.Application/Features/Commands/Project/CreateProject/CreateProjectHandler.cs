using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Domain.Models;
using Blog.Common.Models.Event.Project;
using MediatR;
using Blog.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Application.Commands.Project.CreateProject;

public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, CreateProjectModel>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTagRepository _tagRepository;

    public CreateProjectHandler(IProjectRepository projectRepository, IProjectTagRepository tagRepository)
    {
        _projectRepository = projectRepository;
        _tagRepository = tagRepository;
    }

    public async Task<CreateProjectModel> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        // generate slug
        var slug = SlugHelper.GenerateSlug(request.Title);
        if (await _projectRepository.Get(p => p.Slug == slug).AnyAsync())
            throw new Blog.Common.Infrastructure.Exeptions.BadRequestException("Project with same slug already exists");

    var project = new Blog.Api.Domain.Models.Project
        {
            ID = UlidHelper.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            ShortDescription = request.ShortDescription,
            Content = request.Content,
            FeaturedImage = request.FeaturedImage,
            Images = request.Images,
            TechnologiesUsed = request.TechnologiesUsed != null ? string.Join(',', request.TechnologiesUsed) : null,
            ProjectUrl = request.ProjectUrl,
            GithubUrl = request.GithubUrl,
            DemoUrl = request.DemoUrl,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = (Blog.Api.Domain.Models.ProjectStatus)request.Status,
            Type = (Blog.Api.Domain.Models.ProjectType)request.Type,
            ClientName = request.ClientName,
            IsFeatured = request.IsFeatured,
            IsPublic = request.IsPublic,
            Order = request.Order,
            Slug = slug,
            CreatedDate = DateTime.UtcNow
        };

        // Tags: attach existing tags and create new ones
        if (request.TagIds != null && request.TagIds.Any())
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

        await _projectRepository.AddAsync(project);

        return new CreateProjectModel
        {
            Id = project.ID,
            Title = project.Title,
            CreatedAt = project.CreatedDate
        };
    }
}

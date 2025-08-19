using System;
using MediatR;

namespace Blog.Common.Models.RequestModels.Category
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public bool ForceDelete { get; set; } = false; // Delete even if has posts
    }
}
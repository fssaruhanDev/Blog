using System;
using MediatR;

namespace Blog.Common.Models.RequestModels.Pages
{
    public class DeletePageCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}

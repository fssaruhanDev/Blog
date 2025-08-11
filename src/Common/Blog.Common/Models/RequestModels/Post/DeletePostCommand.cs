using System;
using MediatR;

namespace Blog.Common.Models.RequestModels.Post
{
    public class DeletePostCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}

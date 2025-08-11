using System;
using MediatR;

namespace Blog.Common.Models.RequestModels.News
{
    public class DeleteNewsCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}

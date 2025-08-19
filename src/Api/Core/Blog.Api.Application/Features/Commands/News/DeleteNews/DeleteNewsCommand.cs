using MediatR;

namespace Blog.Api.Application.Features.Commands.News.DeleteNews
{
    public class DeleteNewsCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteNewsCommand(Guid id)
        {
            Id = id;
        }
    }
}

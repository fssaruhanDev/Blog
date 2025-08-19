using Blog.Api.Application.Interfaces.Infrastructure.Utility.Logger;
using Blog.Api.Domain.Interfaces.Repositories;
using Blog.Api.Application.Features.Commands.News.DeleteNews;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Api.Application.Features.Commands.News.DeleteNews
{
    public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand, bool>
    {
        private readonly INewsRepository _newsRepository;
        private readonly ILoggerService _loggerService;

        public DeleteNewsCommandHandler(INewsRepository newsRepository, ILoggerService loggerService)
        {
            _newsRepository = newsRepository;
            _loggerService = loggerService;
        }

        public async Task<bool> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            var logProps = new Dictionary<string, object>
            {
                ["NewsId"] = request.Id
            };

            _loggerService.LogInformation("Deleting news.", logProps);

            var news = await _newsRepository.GetByIdAsync(request.Id);
            if (news == null)
            {
                _loggerService.LogWarning("News deletion failed: News not found.", logProps);
                throw new InvalidOperationException("News not found.");
            }

            await _newsRepository.DeleteAsync(news);

            _loggerService.LogInformation("News deleted successfully.", logProps);

            return true;
        }
    }
}

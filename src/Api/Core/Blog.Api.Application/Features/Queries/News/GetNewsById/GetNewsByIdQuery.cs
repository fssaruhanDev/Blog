using System;
using Blog.Common.Models.Queries.News;
using MediatR;

namespace Blog.Api.Application.Features.Queries.News.GetNewsById
{
    public class GetNewsByIdQuery : IRequest<NewsListItemViewModel>
    {
        public Guid Id { get; set; }
        public bool PublicOnly { get; set; } = false;
    }
}

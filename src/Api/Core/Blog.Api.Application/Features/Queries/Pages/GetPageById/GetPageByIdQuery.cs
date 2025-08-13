using System;
using Blog.Common.Models.Queries.Pages;
using MediatR;

namespace Blog.Api.Application.Features.Queries.Pages.GetPageById
{
    public class GetPageByIdQuery : IRequest<PageListItemViewModel>
    {
        public Guid Id { get; set; }
        public bool PublicOnly { get; set; } = false;
    }
}

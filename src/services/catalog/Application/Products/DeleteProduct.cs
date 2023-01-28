using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Catalog.Application.Data;
using FluentValidation;
using FSH.Core.Dto;
using FSH.Core.Mediator;
using MongoDB.Driver;

namespace Catalog.Application.Products;

public static class DeleteProduct
{
    //Request
    public record Request(Guid Id) : ICommand<Response>;

    //Response
    public record Response(Guid Id) : IDto;

    //Handler
    public class Handler : ICommandHandler<Request, Response>
    {
        private readonly CatalogDbContext _context;

        public Handler(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var result = await _context.Products.DeleteOneAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (result.DeletedCount == 0) throw new ProductNotFoundException(request.Id);
            return new Response(request.Id);
        }
    }
}

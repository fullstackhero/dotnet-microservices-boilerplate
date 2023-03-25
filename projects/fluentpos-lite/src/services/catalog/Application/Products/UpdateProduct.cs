using Ardalis.GuardClauses;
using FluentPOS.Lite.Catalog.Application.Data;
using FSH.Core.Common;
using FSH.Core.Dto;
using FSH.Core.Mediator;
using MongoDB.Driver;

namespace FluentPOS.Lite.Catalog.Application.Products;

public static class UpdateProduct
{
    //Request
    public record Request(Guid Id, string Name, decimal Price) : ICommand<Response>;

    //Response
    public record Response(Guid Id) : IDto;

    //Handler
    public class Handler : ICommandHandler<Request, Response>
    {
        private readonly CatalogDbContext _context;
        private readonly IAuthenticatedUser _user;

        public Handler(CatalogDbContext context, IAuthenticatedUser user)
        {
            _context = context;
            _user = user;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var product = _context.Products.AsQueryable().FirstOrDefault(a => a.Id == request.Id);
            if (product is null) throw new ProductNotFoundException(request.Id);
            product.LastModifiedBy = _user.Id;
            product.LastModifiedOn = DateTime.UtcNow;
            var updatedProduct = product.Update(request.Name, request.Price);
            _ = await _context.Products.ReplaceOneAsync(x => x.Id == request.Id, updatedProduct, cancellationToken: cancellationToken);
            return new Response(product.Id);
        }
    }
}

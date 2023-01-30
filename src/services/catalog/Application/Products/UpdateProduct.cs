using Ardalis.GuardClauses;
using Catalog.Application.Data;
using FSH.Core.Dto;
using FSH.Core.Mediator;
using MongoDB.Driver;

namespace Catalog.Application.Products;

public static class UpdateProduct
{
    //Request
    public record Request(Guid Id, string Name, int Quantity, decimal Price) : ICommand<Response>;

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
            var product = _context.Products.AsQueryable().FirstOrDefault(a => a.Id == request.Id);
            if (product is null) throw new ProductNotFoundException(request.Id);
            var updatedProduct = product.Update(request.Name, request.Quantity, request.Price);
            _ = await _context.Products.ReplaceOneAsync(x => x.Id == request.Id, updatedProduct, cancellationToken: cancellationToken);
            return new Response(product.Id);
        }
    }
}

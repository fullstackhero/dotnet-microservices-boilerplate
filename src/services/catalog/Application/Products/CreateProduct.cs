using Ardalis.GuardClauses;
using Catalog.Application.Data;
using Catalog.Domain.Entities;
using FluentValidation;
using FSH.Core.Dto;
using FSH.Core.Mediator;

namespace Catalog.Application.Products;

public class CreateProduct
{
    //Request
    public record Request(string Name, int Quantity, decimal Price) : ICommand<Response>;

    //Response
    public record Response(Guid id) : IDto;

    //Validator
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(75);
            RuleFor(p => p.Price).GreaterThanOrEqualTo(1);
        }
    }

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
            var product = Product.Create(request.Name, request.Quantity, request.Price);
            await _context.Products.InsertOneAsync(product);
            return new Response(product.Id);
        }
    }
}

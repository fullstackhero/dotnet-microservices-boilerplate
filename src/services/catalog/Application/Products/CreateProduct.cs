using Ardalis.GuardClauses;
using Catalog.Domain.Entities;
using FluentValidation;
using FSH.Core.Dto;
using FSH.Core.Mediator;

namespace Catalog.Application.Products;

public static class CreateProduct
{
    //Request
    public record Request(string Name, int Quantity, decimal Price) : ICommand<Response>;

    //Response
    public record Response(Guid Id) : IDto;

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
    internal class Handler : ICommandHandler<Request, Response>
    {
        public Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            var product = Product.Create(request.Name, request.Quantity, request.Price);
            return Task.FromResult(new Response(product.Id));
        }
    }
}

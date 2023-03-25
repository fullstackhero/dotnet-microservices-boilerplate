using Ardalis.GuardClauses;
using AutoMapper;
using FluentPOS.Lite.Catalog.Application.Data;
using FluentPOS.Lite.Catalog.Domain.Entities;
using FluentValidation;
using FSH.Core.Common;
using FSH.Core.Dto;
using FSH.Core.Mediator;
using MongoDB.Driver;

namespace FluentPOS.Lite.Catalog.Application.Products;

public static class CreateProduct
{
    //Request
    public record Request(string Name, string Details, string Code, decimal Cost, decimal Price, decimal Quantity = 0, decimal AlertQuantity = 10, bool TrackQuantity = true) : ICommand<Response>;

    //Response
    public record Response(Guid Id) : IDto;

    //Validator
    public class Validator : AbstractValidator<Request>
    {
        public Validator(CatalogDbContext context)
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(75);
            RuleFor(p => p.Code).NotEmpty().MaximumLength(10)
                .MustAsync(async (code, ct) => !await context.Products.Find(doc => doc.Code == code).AnyAsync(cancellationToken: ct))
                .WithMessage((_, code) => $"Product with Code : {code} already Exists.");
            RuleFor(p => p.Cost).GreaterThanOrEqualTo(1);
            RuleFor(p => p.Price).GreaterThanOrEqualTo(p => p.Cost);
        }
    }

    //Handler
    public class Handler : ICommandHandler<Request, Response>
    {
        private readonly CatalogDbContext _context;
        private readonly ICacheService _cache;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUser _user;

        public Handler(CatalogDbContext context, ICacheService cache, IMapper mapper, IAuthenticatedUser user)
        {
            _context = context;
            _cache = cache;
            _mapper = mapper;
            _user = user;
        }

        public async Task<Response> Handle(Request req, CancellationToken cancellationToken)
        {
            Guard.Against.Null(req, nameof(req));
            var product = Product.Create(req.Name, req.Details, req.Code, req.Cost, req.Price, req.Quantity, req.AlertQuantity, req.TrackQuantity);
            product.CreatedBy = _user.Id;
            await _context.Products.InsertOneAsync(product, cancellationToken: cancellationToken);
            var productDto = _mapper.Map<ProductDto>(product);
            var cacheKey = Product.GetCacheKey(product.Id);
            await _cache.SetAsync(cacheKey, productDto, cancellationToken: cancellationToken);
            return new Response(product.Id);
        }
    }
}

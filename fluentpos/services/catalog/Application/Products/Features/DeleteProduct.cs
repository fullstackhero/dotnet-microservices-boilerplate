using FluentPos.Catalog.Domain.Products;
using FSH.Framework.Core.Caching;
using MediatR;

namespace FluentPos.Catalog.Application.Products.Features;
public static class DeleteProduct
{
    public sealed record Command : IRequest
    {
        public readonly Guid Id;
        public Command(Guid id)
        {
            Id = id;
        }
    }
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService _cacheService;

        public Handler(IProductRepository repository, ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await _repository.DeleteByIdAsync(request.Id, cancellationToken);
            await _cacheService.RemoveAsync(Product.GetCacheKey(request.Id), cancellationToken);
        }
    }
}

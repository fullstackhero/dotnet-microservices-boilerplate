using Ardalis.GuardClauses;
using AutoMapper;
using FluentPOS.Lite.Catalog.Application.Data;
using FluentPOS.Lite.Catalog.Domain.Entities;
using FluentValidation;
using FSH.Core.Common;
using FSH.Core.Mediator;
using FSH.Infrastructure.Pagination;
using FSH.Persistence.MongoDb;
using MongoDB.Driver;

namespace FluentPOS.Lite.Catalog.Application.Products;

public static class GetProducts
{
    //Request
    public record Request(int PageNumber, int PageSize) : ICommand<PagedList<ProductDto>>;

    //Handler
    public class Handler : ICommandHandler<Request, PagedList<ProductDto>>
    {
        private readonly CatalogDbContext _context;
        private readonly IMapper _mapper;

        public Handler(CatalogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<ProductDto>> Handle(Request request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            return await _context
                            .Products
                            .AsQueryable()
                            .ApplyPagingAsync<Product, ProductDto>(_mapper.ConfigurationProvider, request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}

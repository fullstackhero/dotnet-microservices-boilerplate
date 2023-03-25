using FluentPOS.Lite.Cart.Host.Models;
using FSH.Persistence.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FluentPOS.Lite.Cart.Data;

public class CartDbContext : MongoDbContext
{
    public IMongoCollection<CartDetail> CartDetails { get; }
    public CartDbContext(IOptions<MongoOptions> options) : base(options)
    {
        CartDetails = GetCollection<CartDetail>();
    }
}

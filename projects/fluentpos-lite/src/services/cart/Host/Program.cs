using FluentPOS.Lite.Cart.Data;
using FluentPOS.Lite.Cart.Host.Models;
using FSH.Infrastructure;
using FSH.Infrastructure.Authentication;
using FSH.Infrastructure.Caching;
using FSH.Infrastructure.Logging.Serilog;
using FSH.Infrastructure.Swagger;
using FSH.Persistence.MongoDb;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var appName = builder.RegisterSerilog();

builder.Services.AddDaprClient();
builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.CustomSchemaIds(type => type.ToString()));

builder.Services.AddOptions();
builder.Services.AddMongoDbContext<CartDbContext>(builder.Configuration);

builder.Services.RegisterJWTAuthentication();
builder.Services.RegisterSwagger(appName);
builder.Services.AddCaching().AddInfrastructureServices();
///
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
///
app.ConfigureSerilog();
app.ConfigureSwagger();
///
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapSubscribeHandler();
app.MapGet("/", () => "Hello From Cart Service!").RequireAuthorization();


// Add to Cart

app.MapPost("/", async (CartDbContext context, [FromBody] CartDetail request) =>
{
    //Assume each customer will have only one cart
    //Thus customer ID can be cart Id
    //Check if cart if already present

    var cart = await context.CartDetails.Find(c => c.CartId == request.CartId).FirstOrDefaultAsync();
    if (cart is null && request.CartItems != null)
    {
        // Create a cart with details
        var newCart = new CartDetail() { CartId = request.CartId, CartItems = new List<CartItem>() };
        foreach (CartItem item in request.CartItems)
        {
            newCart.CartItems.Add(item);
        }
        await context.CartDetails.InsertOneAsync(newCart);
        return Results.Created("carts", "added new cart");
    }
    else if (cart is not null && request.CartItems != null)
    {
        foreach (CartItem item in request.CartItems)
        {
            var existingItem = cart.CartItems!.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.CartItems!.Add(item);
            }
        }
        _ = await context.CartDetails.ReplaceOneAsync(x => x.CartId == request.CartId, cart);
        return Results.Created("carts", "updated existing cart");
    }
    else
    {
        return Results.BadRequest();
    }

});

app.Run();

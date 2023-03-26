using System.Reflection;
using FluentPOS.Lite.Cart.Data;
using FluentPOS.Lite.Cart.Host.Carts;
using FSH.Core.Mediator;
using FSH.Infrastructure;
using FSH.Infrastructure.Authentication;
using FSH.Infrastructure.Caching;
using FSH.Infrastructure.Logging.Serilog;
using FSH.Infrastructure.Swagger;
using FSH.Infrastructure.Validations;
using FSH.Persistence.MongoDb;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

var assembly = typeof(Program).GetTypeInfo().Assembly;
builder.Services.AddAutoMapper(assembly);
builder.Services.RegisterMediatR(assembly);
builder.Services.RegisterValidators(assembly);
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

app.MapPost("/", async (IMediator mediator, [FromBody] CreateUpdateCart.Request request, CancellationToken cancellationToken) =>
{
    return await mediator.Send(request, cancellationToken);
}).RequireAuthorization();

app.MapGet("/", async (IMediator mediator, int pageNumber, int pageSize, CancellationToken cancellationToken) =>
{
    return await mediator.Send(new GetCarts.Request(pageNumber, pageSize), cancellationToken);
}).RequireAuthorization();

app.MapGet("/{customerId}", async (IMediator mediator, Guid customerId, CancellationToken cancellationToken) =>
{
    return await mediator.Send(new GetCartDetails.Request(customerId), cancellationToken);
}).RequireAuthorization();

app.Run();

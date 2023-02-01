using System.Reflection;
using Catalog.Application;
using Catalog.Application.Data;
using FSH.Core.Mediator;
using FSH.Infrastructure;
using FSH.Infrastructure.Authentication;
using FSH.Infrastructure.Caching;
using FSH.Infrastructure.Logging.Serilog;
using FSH.Infrastructure.Swagger;
using FSH.Infrastructure.Validations;
using FSH.Persistence.MongoDb;

var builder = WebApplication.CreateBuilder(args);
var appName = builder.RegisterSerilog();

builder.Services.AddDaprClient();
builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions();
builder.Services.AddMongoDbContext<CatalogDbContext>(builder.Configuration);

///
var assembly = typeof(CatalogApplicationRoot).GetTypeInfo().Assembly;
builder.Services.RegisterJWTAuthentication();
builder.Services.AddAutoMapper(assembly);
builder.Services.RegisterMediatR(assembly);
builder.Services.RegisterSwagger(appName);
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
app.MapGet("/", () => "Hello From Catalog Service!").RequireAuthorization();
app.Run();

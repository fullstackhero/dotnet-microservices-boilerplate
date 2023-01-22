using FSH.Infrastructure.Logging.Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterSerilog();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.ConfigureSerilog();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Hello From Catalog Service!");
app.Run();
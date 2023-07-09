using Microsoft.EntityFrameworkCore;
using shoppingify;
using shoppingify.Entities;
using shoppingify.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ShoppingContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapGet("/db", async (ShoppingContext context) =>
{
    await context.Database.EnsureCreatedAsync();
    return Results.Ok("Database created");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
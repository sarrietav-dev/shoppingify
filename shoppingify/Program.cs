using shoppingify.Cart.Domain;
using shoppingify.Cart.Infrastructure.Repositories;
using Serilog;
using shoppingify.Middleware;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using shoppingify.IAM.Application;
using shoppingify.IAM.Infrastructure;
using shoppingify;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(x =>
    {
        x.ClearProviders();
        x.AddSerilog(dispose: true);
    }
);
builder.Services.AddScoped<ICartOwnerRepository, MockCartOwnerRepository>();
builder.Services.AddScoped<IAuthenticationProviderService, FakeAuthenticationProvider>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("shoppingify");
});

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.GetApplicationDefault(),
    ProjectId = "shoppingify-6c574"
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<AuthTokenMiddleware>();

app.Run();
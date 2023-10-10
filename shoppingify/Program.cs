using shoppingify.Cart.Domain;
using Serilog;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using shoppingify.IAM.Application;
using shoppingify.IAM.Infrastructure;
using shoppingify;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using shoppingify.Cart.Application;
using shoppingify.Products.Application;
using shoppingify.Cart.Infrastructure.Persistence;
using shoppingify.Products.Domain;
using shoppingify.Products.Infrastructure.Persistence;

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

builder.Services.AddTransient<ICartOwnerRepository, EFCartOwnerRepository>();
builder.Services.AddTransient<IAuthenticationProviderService, FakeAuthenticationProvider>();
builder.Services.AddTransient<ICartApplicationService, CartApplicationService>();
builder.Services.AddTransient<IProductApplicationService, ProductApplicationService>();
builder.Services.AddTransient<ICartRepository, EFCartRepository>();
builder.Services.AddTransient<IProductRepository, EFProductRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("shoppingify");
});

builder.Services.AddAuthentication()
    .AddScheme<AppAuthenticationSchemeOptions, AppAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, options => { });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
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

app.Run();
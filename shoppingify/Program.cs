using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shoppingify;
using Shoppingify.Cart.Application;
using Shoppingify.Cart.Domain;
using Shoppingify.Cart.Infrastructure.Repositories;
using Shoppingify.IAM.Application;
using Shoppingify.IAM.Infrastructure;
using Shoppingify.Products.Application;
using Shoppingify.Products.Domain;
using Shoppingify.Products.Infrastructure.Persistence;

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

const string myOrigins = "_myOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddTransient<ICartOwnerRepository, EfCartOwnerRepository>();
builder.Services.AddTransient<IAuthenticationProviderService, FakeAuthenticationProvider>();
builder.Services.AddTransient<ICartApplicationService, CartApplicationService>();
builder.Services.AddTransient<IProductApplicationService, ProductApplicationService>();
builder.Services.AddTransient<ICartRepository, EfCartRepository>();
builder.Services.AddTransient<IProductRepository, EfProductRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication()
    .AddScheme<AppAuthenticationSchemeOptions, AppAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme,
        _ => { });


builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

FirebaseApp.Create(new AppOptions
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

app.UseCors(myOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
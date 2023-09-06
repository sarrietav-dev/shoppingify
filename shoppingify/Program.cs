using shoppingify.Cart.Domain;
using shoppingify.Cart.Infrastructure.Repositories;
using shoppingify.IAM.Application;
using shoppingify.IAM.Infrastructure;
using Serilog;

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
builder.Services.AddScoped<IAuthenticationProviderService, MockAuthenticationService>();
builder.Services.AddScoped<IdentityApplicationService, IdentityApplicationService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

/*
FirebaseApp.Create(new AppOptions()
    { Credential = GoogleCredential.GetApplicationDefault(), ProjectId = "shoppingify-6c574" });
*/

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
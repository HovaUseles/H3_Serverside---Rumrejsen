using GalacticRoutesAPI.Managers;
using GalacticRoutesAPI.Middleware;
using GalacticRoutesAPI.Repositories;
using GalacticRoutesAPI.Repositories.Interfaces;
using GalacticRoutesAPI.Seeder;
using GalacticRoutesAPI.Services;
using GalacticRoutesAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injecting Repositories
builder.Services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericMockRepository<>));
builder.Services.AddSingleton<IApiKeyRepository, ApiKeyRepostory>();

// Injecting Services
builder.Services.AddScoped<IApiKeyValidator, ApiKeyValidator>();
builder.Services.AddScoped<IApiKeyGenerator, RandomApiKeyGenerator>();

// Injecting Managers
builder.Services.AddScoped<ApiKeyManager>();
builder.Services.AddScoped<GalacticRouteManager>();
builder.Services.AddScoped<RequestManager>();
builder.Services.AddScoped<SpaceTravelerManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// If in development seed database
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        Seeder seeder = new Seeder();
        seeder.SeedMockDb(
            scope.ServiceProvider.GetRequiredService<GalacticRouteManager>(),
            scope.ServiceProvider.GetRequiredService<SpaceTravelerManager>()
            );
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//registering DbContext
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Adding Repository as Service
builder.Services.AddScoped<IProductRepository, ProductRepository>();

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

app.MapControllers();

//we want to automate the migrations whenever the application starts
//and make data up-to-date
try
{
    // Create a scope(temp container by which we can use Services through DI) to access application services like DbContext
    using var scope = app.Services.CreateScope();

    // Get the service provider(place where all dependencies get registered) from the scope
    var services = scope.ServiceProvider;

    // Get the StoreContext (our DbContext) through DI to interact with the database
    var context = services.GetRequiredService<StoreContext>();

    // Apply any pending migrations to the database (auto-update schema)
    await context.Database.MigrateAsync();

    // Seed initial data to the database if it’s empty
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
	throw;
}

app.Run();

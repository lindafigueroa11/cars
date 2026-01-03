using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Backend.Services;
using Backend.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =====================
// DATABASE
// =====================

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<StoreContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("StoreConnection")
        ));
}
else
{
    builder.Services.AddDbContext<StoreContext>(options =>
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("StoreConnection")
        ));
}

// =====================
// DEPENDENCY INJECTION
// =====================

// Repository
builder.Services.AddScoped<IRepository<Car>, CarRepository>();

// Keyed Service (OBLIGATORIO)
builder.Services.AddKeyedScoped<
    ICommonService<CarDTOs, CarInsertDTOs, CarUpdateDTOs>,
    CarService
>("carService");

// Validators
builder.Services.AddScoped<IValidator<CarInsertDTOs>, CarInsertValidator>();
builder.Services.AddScoped<IValidator<CarUpdateDTOs>, CarUpdateValidator>();

// =====================
// MVC + SWAGGER
// =====================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =====================
// BUILD
// =====================

var app = builder.Build();

// =====================
// MIDDLEWARE
// =====================

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

// =====================
// MIGRATIONS
// =====================

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StoreContext>();
    db.Database.Migrate();
}

// =====================
// RUN
// =====================

app.Run();

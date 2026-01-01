using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Backend.Services;
using Backend.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =====================
// SERVICIOS
// =====================

// Servicios propios
builder.Services.AddSingleton<ICocheService, CocheService>();

builder.Services.AddHttpClient<IPostsService, PostsService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["BaseUrlPost"]);
});

builder.Services.AddScoped<IRepository<Car>, CarRepository>();

builder.Services.AddKeyedScoped<
    ICommonService<CarDTOs, CarInsertDTOs, CarUpdateDTOs>,
    CarService>("carService");

// DbContext con PostgreSQL (Render)
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("StoreConnection")
    );
});

// Controllers
builder.Services.AddControllers();

// FluentValidation
builder.Services.AddScoped<IValidator<CarInsertDTOs>, CarInsertValidator>();
builder.Services.AddScoped<IValidator<CarUpdateDTOs>, CarUpdateValidator>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =====================
// BUILD
// =====================

var app = builder.Build();

// =====================
// PIPELINE
// =====================

// 🔥 Swagger SIEMPRE activo (Render = Production)
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS redirection (opcional en Render, no rompe nada)
app.UseHttpsRedirection();

app.UseAuthorization();

// 🔑 Mapea los controllers (OBLIGATORIO)
app.MapControllers();

// =====================
// RUN
// =====================

app.Run();

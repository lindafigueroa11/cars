using Backend.Models;
using Backend.Services;
using Backend.Repository;
using Backend.DTOs;
using Backend.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StoreContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("StoreConnection"));
    }
    else
    {
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("StoreConnection"));
    }
});

builder.Services.AddScoped<IRepository<Car>, CarRepository>();
builder.Services.AddKeyedScoped<ICommonService<CarDTOs, CarInsertDTOs, CarUpdateDTOs>, CarService>("carService");

builder.Services.AddScoped<IValidator<CarInsertDTOs>, CarInsertValidator>();
builder.Services.AddScoped<IValidator<CarUpdateDTOs>, CarUpdateValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();

using Backend.Models;
using Backend.Services;
using Backend.Repository;
using Backend.DTOs;
using Backend.Validators;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

/* =======================
   DATABASE
======================= */
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("StoreConnection")
    )
);

/* =======================
   REPOSITORIES
======================= */
builder.Services.AddScoped<IRepository<Car>, CarRepository>();
builder.Services.AddScoped<CarLocationRepository>();

/* =======================
   SERVICES
======================= */
builder.Services.AddScoped<CarService>();
builder.Services.AddScoped<CarLocationService>();
builder.Services.AddHttpClient<ReverseGeocodingService>();
builder.Services.AddScoped<IImageService, ImageService>();

/* =======================
   VALIDATORS
======================= */
builder.Services.AddScoped<IValidator<CarInsertDTOs>, CarInsertValidator>();
builder.Services.AddScoped<IValidator<CarUpdateDTOs>, CarUpdateValidator>();

/* =======================
   CLOUDINARY
======================= */
builder.Services.AddSingleton(sp =>
{
    var cloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
    var apiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY");
    var apiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");

    if (string.IsNullOrEmpty(cloudName) ||
        string.IsNullOrEmpty(apiKey) ||
        string.IsNullOrEmpty(apiSecret))
    {
        throw new Exception("Cloudinary environment variables are missing");
    }

    return new Cloudinary(new Account(cloudName, apiKey, apiSecret));
});

/* =======================
   CONTROLLERS & SWAGGER
======================= */
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/* =======================
   MIDDLEWARE
======================= */
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

/* =======================
   MIGRATIONS AUTO APPLY
======================= */
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StoreContext>();
    db.Database.Migrate();
}

app.Run();

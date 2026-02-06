using Backend.Models;
using Backend.Services;
using Backend.DTOs;
using Backend.Validators;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using System.Text.Json.Serialization;

Console.WriteLine("=== APP STARTING ===");

var builder = WebApplication.CreateBuilder(args);

/* =======================
   DATABASE
======================= */

var connectionString =
    Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("StoreConnection")
    ?? throw new Exception("No database connection string configured");

builder.Services.AddDbContext<StoreContext>(options =>
    options.UseNpgsql(connectionString)
);

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
   CONTROLLERS & JSON
======================= */
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* =======================
   CORS
======================= */
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

/* =======================
   AUTO DB FIX (FOR DEVELOPMENT)
======================= */
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<StoreContext>();

        db.Database.ExecuteSqlRaw("""
            ALTER TABLE "Cars"
            ADD COLUMN IF NOT EXISTS "Price" numeric NOT NULL DEFAULT 0;
        """);

        db.Database.ExecuteSqlRaw("""
            ALTER TABLE "Cars"
            ADD COLUMN IF NOT EXISTS "IsAutomatic" boolean;
        """);

        db.Database.ExecuteSqlRaw("""
            ALTER TABLE "Cars"
            ADD COLUMN IF NOT EXISTS "Color" text;
        """);

        db.Database.ExecuteSqlRaw("""
            ALTER TABLE "Cars"
            ADD COLUMN IF NOT EXISTS "PublishedAt" timestamp with time zone;
        """);

        db.Database.ExecuteSqlRaw("""
            ALTER TABLE "Cars"
            ADD COLUMN IF NOT EXISTS "ImageUrl" text;
        """);

        db.Database.Migrate();

        Console.WriteLine("=== DATABASE READY ===");
    }
    catch (Exception ex)
    {
        Console.WriteLine("=== DATABASE INIT ERROR ===");
        Console.WriteLine(ex.ToString());
    }
}

/* =======================
   MIDDLEWARE
======================= */
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();

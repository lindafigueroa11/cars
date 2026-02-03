using Backend.Models;
using Backend.Services;
using Backend.DTOs;
using Backend.Validators;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

/* =======================
   DATABASE (Render ready)
======================= */

var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
string connectionString;

if (!string.IsNullOrEmpty(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');

    connectionString =
        $"Host={uri.Host};" +
        $"Port={uri.Port};" +
        $"Database={uri.AbsolutePath.Trim('/')};" +
        $"Username={userInfo[0]};" +
        $"Password={userInfo[1]};" +
        $"SSL Mode=Require;" +
        $"Trust Server Certificate=true";
}
else
{
    connectionString = builder.Configuration.GetConnectionString("StoreConnection")
        ?? throw new Exception("No database connection string configured");
}

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
   CORS (safe default)
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
   MIDDLEWARE
======================= */
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

/* =======================
   AUTO MIGRATIONS
======================= */
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<StoreContext>();
        db.Database.Migrate();
        Console.WriteLine("Database migrated successfully");
    }
    catch (Exception ex)
    { 
        Console.WriteLine("Database migration skipped:");
        Console.WriteLine(ex.Message);
    }
}

 

app.Run();

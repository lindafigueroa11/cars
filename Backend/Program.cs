using Backend.Models;
using Backend.Services;
using Backend.Repository;
using Backend.DTOs;
using Backend.Validators;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

#region Database
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("StoreConnection")
    )
);
#endregion

#region Repositories
builder.Services.AddScoped<IRepository<Car>, CarRepository>();
#endregion

#region Services
builder.Services.AddKeyedScoped<
    ICommonService<CarDTOs, CarInsertDTOs, CarUpdateDTOs>,
    CarService
>("carService");
#endregion

#region Validators
builder.Services.AddScoped<IValidator<CarInsertDTOs>, CarInsertValidator>();
builder.Services.AddScoped<IValidator<CarUpdateDTOs>, CarUpdateValidator>();
builder.Services.AddScoped<CarLocationRepository>();
builder.Services.AddScoped<CarLocationService>();
IHttpClientBuilder httpClientBuilder = builder.Services.AddHttpClient<ReverseGeocodingService>();

IServiceCollection serviceCollection = builder.Services.AddScoped<IImageService, ImageService>();
#endregion

#region Cloudinary
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

    var account = new Account(cloudName, apiKey, apiSecret);
    return new Cloudinary(account);
});
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

#region Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
#endregion

#region Database Migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StoreContext>();
    db.Database.Migrate();
}
#endregion

app.Run();

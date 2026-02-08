using System.Text;
using System.Text.Json.Serialization;
using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Backend.Repository.Interfaces;
using Backend.Services;
using Backend.Services.Interfaces;
using Backend.Validators;
using CloudinaryDotNet;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
   JWT AUTH
======================= */
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

/* =======================
   SERVICES
======================= */
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<CarService>();
builder.Services.AddScoped<CarLocationService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddHttpClient<ReverseGeocodingService>();

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
        throw new Exception("Cloudinary env vars missing");

    return new Cloudinary(new Account(cloudName, apiKey, apiSecret));
});

/* =======================
   CONTROLLERS
======================= */
builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* =======================
   CORS
======================= */
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAll", p =>
        p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

/* =======================
   MIDDLEWARE
======================= */
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

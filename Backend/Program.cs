using Backend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StoreContext>();
    db.Database.Migrate();
}

app.Run();

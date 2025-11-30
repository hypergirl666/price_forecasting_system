using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PriceForecasting.Data.Context;

var builder = WebApplication.CreateBuilder(args);

// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGitHubPages", policy =>
    {
        policy.WithOrigins(
                "https://urfu-priceforecast.github.io",
                "http://localhost:3000", 
                "https://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PriceForecasting API", Version = "v1" });
});

// ✅ ВРЕМЕННО: In-Memory база (уберите когда добавите Npgsql)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("PriceForecastingDB"));

/*
// ✅ ДЛЯ POSTGRESQL (когда установите Npgsql):
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}
else
{
    connectionString = ConvertDatabaseUrlToConnectionString(connectionString);
}
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

static string ConvertDatabaseUrlToConnectionString(string databaseUrl)
{
    var uri = new Uri(databaseUrl);
    var host = uri.Host;
    var port = uri.Port;
    var database = uri.AbsolutePath.Trim('/');
    var user = uri.UserInfo.Split(':')[0];
    var password = uri.UserInfo.Split(':')[1];
    return $"Host={host};Port={port};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";
}
*/

builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseCors("AllowGitHubPages");

// ✅ ПРОСТЫЕ ЭНДПОИНТЫ
app.MapGet("/", () => "PriceForecasting API is running!");
app.MapGet("/api/health", () => new { status = "OK", message = "API is working" });

// ✅ ТЕСТОВЫЕ ДАННЫЕ ДЛЯ In-Memory
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Добавьте тестовые данные...
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

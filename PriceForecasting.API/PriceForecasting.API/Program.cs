using Microsoft.OpenApi.Models;
// using Microsoft.EntityFrameworkCore; // ЗАКОММЕНТИРОВАТЬ
// using PriceForecasting.Data.Context; // ЗАКОММЕНТИРОВАТЬ

var builder = WebApplication.CreateBuilder(args);

// CORS
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

// ✅ ЗАКОММЕНТИРОВАТЬ ВСЁ С БАЗОЙ ДАННЫХ
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseInMemoryDatabase("PriceForecastingDB"));

builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseCors("AllowGitHubPages");

// ✅ ПРОСТЫЕ ЭНДПОИНТЫ БЕЗ БД
app.MapGet("/", () => "PriceForecasting API is running!");
app.MapGet("/api/health", () => new { status = "OK", message = "API is working" });

app.MapGet("/api/test/products/{id}", (string id) => 
    new { 
        id = id,
        name = $"Тестовый товар {id}",
        price = 2999.99m,
        category = "Электроника",
        date = DateTime.Now.ToString("yyyy-MM-dd")
    });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

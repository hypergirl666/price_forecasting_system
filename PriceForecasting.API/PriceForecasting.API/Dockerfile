FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# 1. Копируем ВСЕ файлы .csproj
COPY ["PriceForecasting.API/PriceForecasting.API/PriceForecasting.API.csproj", "PriceForecasting.API/PriceForecasting.API/"]
COPY ["PriceForecasting.Core/PriceForecasting.Core.csproj", "PriceForecasting.Core/"]
COPY ["PriceForecasting.Data/PriceForecasting.Data.csproj", "PriceForecasting.Data/"]
COPY ["PriceForecastingSystem.sln", "."]

# 2. Восстанавливаем зависимости основного проекта
RUN dotnet restore "PriceForecasting.API/PriceForecasting.API/PriceForecasting.API.csproj"

# 3. Копируем ВЕСЬ исходный код
COPY . .

# 4. Переходим в папку API и собираем
WORKDIR "/src/PriceForecasting.API/PriceForecasting.API"
RUN dotnet build "PriceForecasting.API.csproj" -c Release -o /app/build

# 5. Публикуем
RUN dotnet publish "PriceForecasting.API.csproj" -c Release -o /app/publish

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PriceForecasting.API.dll"]
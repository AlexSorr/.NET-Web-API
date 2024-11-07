# Указываем базовый образ для запуска приложения (ASP.NET Core Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Этап сборки (SDK образ для сборки и публикации)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл проекта и выполняем восстановление зависимостей
COPY ["TestWebAPI/TestWebAPI.csproj", "TestWebAPI/"]
RUN dotnet restore "TestWebAPI/TestWebAPI.csproj"

# Копируем все файлы и выполняем сборку
COPY . .
WORKDIR "/src/TestWebAPI"
RUN dotnet build "TestWebAPI.csproj" -c Release -o /app/build

# Этап публикации приложения
FROM build AS publish
RUN dotnet publish "TestWebAPI.csproj" -c Release -o /app/publish

# Финальный этап — создание образа с приложением
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestWebAPI.dll"]

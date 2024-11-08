using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

using API.Data;

var builder = WebApplication.CreateBuilder(args);

// Настройка Serilog для логирования в файл
Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> configureLogging = (context, services, configuration) => {
    string? logFilePath = context.Configuration["Logging:File:Path"];
    if (string.IsNullOrWhiteSpace(logFilePath))
        return;
    configuration.WriteTo.Console().WriteTo.File(path: logFilePath, rollingInterval: RollingInterval.Day); 
};
builder.Host.UseSerilog(configureLogging);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Конфигурация пайплайна HTTP запросов
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

// Добавление сервисов
void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))//);
        .UseLazyLoadingProxies()); //Включаем для ленивой загрузки виртуальных ICollection, чтобы не тащить много лишних данных сразу
    services.AddControllers();
    services.AddEndpointsApiExplorer(); // Добавление эндпоинтов и конфигурации для Swagger
    services.AddSwaggerGen();
    //services.AddSwaggerGen(c => c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" }));

    //загрузчик данных
    services.AddTransient<DataLoader>();
}


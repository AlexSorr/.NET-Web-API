using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Настройка Serilog для логирования в файл
Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> configureLogging = (context, services, configuration) => {
    string? logFilePath = context.Configuration["Logging:File:Path"];
    if (string.IsNullOrWhiteSpace(logFilePath))
        return;
    configuration.WriteTo.Console().WriteTo.File(path: logFilePath, rollingInterval: RollingInterval.Day); 
};
builder.Host.UseSerilog(configureLogging);

// Вызов метода ConfigureServices для добавления зависимостей
ConfigureServices(builder.Services, builder.Configuration);

// Добавление эндпоинтов и конфигурации для Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapControllers();

// Конфигурация пайплайна HTTP запросов
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

// Метод ConfigureServices для добавления сервисов
void ConfigureServices(IServiceCollection services, IConfiguration configuration) {
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

using API.Data;
using API.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

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
    services.AddSwaggerGen(c => { //не работает для загрузки файлов, это кал
        c.OperationFilter<FileUploadOperationFilter>();
    });

    //загрузчик данных
    services.AddTransient<DataLoader>();

    // Регистрация EntityService как обобщенного сервиса
    services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));

}

/// <summary>
/// Фильтр для загрузки файлов в Swagger !!!Не работает
/// </summary>
public class FileUploadOperationFilter : IOperationFilter {
    public void Apply(OpenApiOperation operation, OperationFilterContext context) {
        var fileParams = context.MethodInfo
            .GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(IFormFileCollection));

        foreach (var param in fileParams) {
            operation.Parameters.Remove(operation.Parameters.First(p => p.Name == param.Name));

            operation.RequestBody = new OpenApiRequestBody {

                Content = new Dictionary<string, OpenApiMediaType> {

                    ["multipart/form-data"] = new OpenApiMediaType {
                        Schema = new OpenApiSchema {
                            Type = "object",
                            Properties = { [param.Name] = new OpenApiSchema { Type = "string", Format = "binary" } },
                        }
                    }
                }
            };
            
        }
    }
}



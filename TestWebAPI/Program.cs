using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

using dNetAPI.Services.Base;
using dNetAPI.Services.Messaging;

WebApplication app = BuildApp(WebApplication.CreateBuilder(args));

ConfigureHttpPipelines(app);
app.MapControllers();
app.Run();

#region AppConfiguration

WebApplication BuildApp(WebApplicationBuilder builder) {
    ConfigureApp(builder.Services, builder.Configuration);
    ConfigureLogging(builder);
    return builder.Build();
}

// Конфигурация пайплайна HTTP запросов
void ConfigureHttpPipelines(WebApplication application) {
    if (app.Environment.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //c.RoutePrefix = string.Empty; //доступ к Swagger UI по корневому URL
        });
    }
}

// Настройка Serilog для логирования в файл
void ConfigureLogging(WebApplicationBuilder builder) {

    Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> configureLogging = (context, services, configuration) => {

        string logFilePath = context.Configuration["Logging:File:Path"] ?? throw new ArgumentNullException("Log file path can not be empty");
        if (string.IsNullOrWhiteSpace(logFilePath)) return;

        configuration.WriteTo.Console().WriteTo.File(path: logFilePath, rollingInterval: RollingInterval.Day); 
        configuration.MinimumLevel.Debug();

    };

    builder.Host.UseSerilog(configureLogging);
}

// Добавление сервисов
void ConfigureApp(IServiceCollection services, IConfiguration configuration) {
    services.ApplyDatabaseContext(configuration);
    services.ConfigureControllers();
    services.AddCustomServices();
    services.ConfigureUI();
}

/// <summary>
/// Класс расширений для <see cref="IServiceCollection"/>, предоставляющий методы настройки сервисов приложения.
/// </summary>
public static class ServiceCollectionExtension {

    /// <summary>
    /// Добавляет и настраивает контекст базы данных с использованием PostgreSQL.
    /// </summary>
    /// <param name="services">Коллекция сервисов для конфигурации.</param>
    /// <param name="configuration">Конфигурация приложения с настройками подключения.</param>
    public static void ApplyDatabaseContext(this IServiceCollection services, IConfiguration configuration) {
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        //.UseLazyLoadingProxies());
    }

    /// <summary>
    /// Добавляет контроллеры и настраивает параметры JSON-сериализации.
    /// </summary>
    /// <param name="services">Коллекция сервисов для конфигурации.</param>
    public static void ConfigureControllers(this IServiceCollection services) { 
        services.AddControllers()
            .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve; });
    }

    /// <summary>
    /// Настраивает пользовательский интерфейс приложения, включая документацию Swagger.
    /// </summary>
    /// <param name="services">Коллекция сервисов для конфигурации.</param>
    public static void ConfigureUI(this IServiceCollection services) {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            c.OrderActionsBy(apiDesc => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
        });
    }

    /// <summary>
    /// Регистрирует пользовательские сервисы и фабрики в контейнере зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов для конфигурации.</param>
    public static void AddCustomServices(this IServiceCollection services) {

        services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));
        services.AddScoped<IEntityServiceFactory, EntityServiceFactory>();

        services.AddSingleton<IRabbitMQService, RabbitMQService>();
    }

}

#endregion
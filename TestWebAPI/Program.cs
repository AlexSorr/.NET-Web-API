using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

using API.Services.Base;
using API.Services.EventService;
using API.Services.Messaging;

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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Если хотите доступ к Swagger UI по корневому URL
    });
}

app.MapControllers();

app.Run();

// Добавление сервисов
void ConfigureServices(IServiceCollection services, IConfiguration configuration) {

    //Database
    services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
      //  .UseLazyLoadingProxies()); //Включаем для ленивой загрузки виртуальных ICollection, чтобы не тащить много лишних данных сразу

    //Controllers
    services.AddControllers()
        .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve; });
    
    //UI
    services.AddEndpointsApiExplorer(); // Добавление эндпоинтов и конфигурации для Swagger
    services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            // Настройка для включения XML-комментариев
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            // Упорядочивание действий по контроллеру и методу
            c.OrderActionsBy(apiDesc =>
                $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
        }
    );

    //Регистрация сервисов в DI
    services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));
    services.AddScoped(typeof(IEventService), typeof(EventService));
    services.AddScoped<IEntityServiceFactory, EntityServiceFactory>();

    //services.AddSingleton<ApplicationDbContextFactory>();
    services.AddSingleton(typeof(IRabbitMQService), typeof(RabbitMQService));
}
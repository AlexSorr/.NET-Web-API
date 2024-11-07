using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Вызов метода ConfigureServices для добавления зависимостей
ConfigureServices(builder.Services, builder.Configuration);

// Добавление эндпоинтов и конфигурации для Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

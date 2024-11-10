using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using API.Models;
using System.Reflection;
using API.Models.Base;

/// <summary>
/// Контекст базы данных для работы с сущностями.
/// Содержит настройки для управления сущностями и конфигурацию базы данных.
/// </summary>
public class ApplicationDbContext : DbContext {
    
    private readonly ILogger<ApplicationDbContext> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ApplicationDbContext"/> с заданными параметрами.
    /// </summary>
    /// <param name="options">Параметры конфигурации контекста.</param>
    /// <param name="logger">Логгер для записи информации о работе контекста.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger) : base(options) { 
        _logger = logger; 
    }

    /// <summary>
    /// Таблица событий.
    /// </summary>
    public DbSet<Event> Events { get; set; }

    /// <summary>
    /// Таблица бронирований.
    /// </summary>
    public DbSet<Booking> Bookings { get; set; }

    /// <summary>
    /// Таблица локаций.
    /// </summary>
    public DbSet<Location> Locations { get; set; }

    /// <summary>
    /// Таблица билетов.
    /// </summary>
    public DbSet<Ticket> Tickets { get; set; }

    /// <summary>
    /// Настраивает параметры модели и устанавливает конфигурацию БД.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели для настройки сущностей.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        
        ApplyEntityKeyConfiguration(modelBuilder);
        ApplyForeignKeyConfiguration(modelBuilder);
        SetDbObjectNamesLowerInvariantCase(modelBuilder);
    }

    /// <summary>
    /// Настраивает первичные ключи для всех сущностей, реализующих интерфейс <see cref="IEntity"/>.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели для настройки сущностей.</param>
    private void ApplyEntityKeyConfiguration(ModelBuilder modelBuilder) {
        IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IEntity).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        foreach (Type type in types) 
            modelBuilder.Entity(type).HasKey("Id");
    }

    /// <summary>
    /// Настраивает параметры внешних ключей, включая связь "событие-билет".
    /// </summary>
    /// <param name="modelBuilder">Построитель модели для настройки сущностей.</param>
    private void ApplyForeignKeyConfiguration(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Event>().HasMany(e => e.Tickets)
            .WithOne(t => t.Event)
            .HasForeignKey("EventId")
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Приводит имена таблиц и столбцов к нижнему регистру для поддержания единообразия.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели для настройки сущностей.</param>
    private void SetDbObjectNamesLowerInvariantCase(ModelBuilder modelBuilder) {
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes()) {

            string tableName = entity.GetTableName() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(tableName)) {
                _logger.LogError($"Table name for entity {entity.GetType()} not found");
                continue;
            }
            entity.SetTableName(tableName.ToLowerInvariant());

            foreach (IMutableProperty property in entity.GetProperties())
                property.SetColumnName(property.Name.ToLowerInvariant());
        }
    }
}

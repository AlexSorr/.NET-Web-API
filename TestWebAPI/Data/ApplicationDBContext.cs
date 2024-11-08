using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using API.Models;
using System.Reflection;
using API.Models.Base;

public class ApplicationDbContext : DbContext {
    
    private readonly ILogger<ApplicationDbContext> _logger;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger) : base(options) { _logger = logger; }

    public DbSet<Event> Events { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Location> Locations { get; set; }  
    public DbSet<Ticket> Tickets { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        base.OnModelCreating(modelBuilder);
        
        // Указываем первичные ключи
        ApplyEntityKeyConfiguration(modelBuilder);

        // Приводим имена колонок и таблиц к нижнему регистру без кавычек
        SetDbObjectNamesLowerInvariantCase(modelBuilder);

    }

    /// <summary>
    /// Получаем все сущности, реализующие интерфейс IEntity из Model и добавляем их в ModelBuilder с привязкой PK - Id
    /// </summary>
    private void ApplyEntityKeyConfiguration(ModelBuilder modelBuilder) {
        IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IEntity).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        foreach (Type type in types) 
            modelBuilder.Entity(type).HasKey("Id");
    }

    /// <summary>
    /// Привести имена таблиц и колонок в них к нижнему регистру без кавычек
    /// </summary>
    private void SetDbObjectNamesLowerInvariantCase(ModelBuilder modelBuilder) {
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes()){

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
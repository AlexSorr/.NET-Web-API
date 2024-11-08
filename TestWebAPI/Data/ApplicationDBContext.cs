using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Models;

public class ApplicationDbContext : DbContext {
    
    private readonly ILogger<ApplicationDbContext> _logger;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger) : base(options) { _logger = logger; }

    public DbSet<Event> Events { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    // Начальная загрузка данных
    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        //Не используем кавычки, чтобы имена таблиц в БД были нечувствительны к регистру
        modelBuilder.UseCollation("C");

        // Указываем первичные ключи //8.11 - а че если перед базой их поставить?
        modelBuilder.Entity<Event>().HasKey(e => e.Id);
        modelBuilder.Entity<Booking>().HasKey(b => b.Id);

        // приводим имена колонок и таблиц к нижнему регистру без кавычек
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
        
        base.OnModelCreating(modelBuilder);
    }

}
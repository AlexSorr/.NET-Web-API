using Microsoft.EntityFrameworkCore;

using Models;

public class ApplicationDbContext : DbContext {
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    // Начальная загрузка данных
    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        //Не используем кавычки, чтобы имена таблиц в БД были нечувствительны к регистру
        modelBuilder.UseCollation("C");

        // Указываем первичные ключи //8.11 - а че если перед базой их поставить?
        modelBuilder.Entity<Event>().HasKey(e => e.Id);
        modelBuilder.Entity<Booking>().HasKey(b => b.Id);

        base.OnModelCreating(modelBuilder);

    }

}
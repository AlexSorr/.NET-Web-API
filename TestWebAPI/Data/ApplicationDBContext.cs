using Microsoft.EntityFrameworkCore;

using Models;

public class ApplicationDbContext : DbContext {
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    // Начальная загрузка данных
    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        base.OnModelCreating(modelBuilder);

        // Указываем первичные ключи
        modelBuilder.Entity<Event>().HasKey(e => e.Id);
        modelBuilder.Entity<Booking>().HasKey(b => b.Id);

    }

}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace API.Data.Factories;

/// <summary>
/// Фабрика для создания экземпляров <see cref="ApplicationDbContext"/> в design-time.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext> {

    /// <summary>
    /// Создает экземпляр <see cref="ApplicationDbContext"/> с заданными параметрами.
    /// </summary>
    /// <param name="args">Аргументы командной строки (не используются).</param>
    /// <returns>Новый экземпляр <see cref="ApplicationDbContext"/>.</returns>
    public ApplicationDbContext CreateDbContext(string[] args) {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));

        return new ApplicationDbContext(optionsBuilder.Options, null);
    }
}

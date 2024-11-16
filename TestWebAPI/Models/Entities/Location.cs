using API.Models.Base;

namespace API.Models;

/// <summary>
/// Представляет локацию для мероприятия.
/// Наследуется от <see cref="Entity"/>.
/// </summary>
public class Location : Entity, IParsableEntity<Location> {

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Location"/>.
    /// Конструктор по умолчанию.
    /// </summary>
    public Location() {}

    /// <summary>
    /// Название локации.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Адрес локации.
    /// </summary>
    public string Address { get; set; } = string.Empty;

     /// <inheritdoc/>
    public static Location Parse(object? @object) {
        return @object as Location;
    }

    /// <inheritdoc/>
    public static bool TryParse(object? @object, out Location? result) {
        result = @object as Location;
        return result != null;
    }
}

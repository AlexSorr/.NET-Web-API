using API.Models.Base;

namespace API.Models;

/// <summary>
/// Представляет локацию для мероприятия.
/// Наследуется от <see cref="Entity"/>.
/// </summary>
public class Location : Entity {

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
}

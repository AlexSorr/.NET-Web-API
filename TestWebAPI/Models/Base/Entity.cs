using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Base;

/// <summary>
/// Абстрактный класс, реализующий интерфейс <see cref="IEntity"/>. 
/// Используется как базовый класс для всех сущностей с идентификатором и датами создания и изменения.
/// </summary>
public abstract class Entity : IEntity {

    /// <summary>
    /// Инициализирует новый экземпляр сущности с установкой даты создания на текущее время.
    /// </summary>
    public Entity() { }

    /// <summary>
    /// Идентификатор сущности. Генерируется автоматически в базе данных.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; }

    /// <summary>
    /// Дата создания сущности.
    /// </summary>
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата последнего изменения сущности. Может быть <c>null</c>, если сущность еще не была изменена.
    /// </summary>
    public DateTime? ChangeDate { get; set; }

    /// <summary>
    /// Переопределение метода <see cref="Equals(object?)"/> для сравнения сущностей по их идентификатору.
    /// </summary>
    /// <param name="obj">Объект для сравнения</param>
    /// <returns>Возвращает <c>true</c>, если объекты идентичны по типу и идентификатору.</returns>
    public override bool Equals(object? obj) {
        if (ReferenceEquals(this, obj)) return true;
        if (obj == null || GetType() != obj.GetType()) return false;
        return Id == ((Entity)obj).Id;
    }

    /// <summary>
    /// Переопределение метода <see cref="GetHashCode"/> для генерации хэш-кода сущности.
    /// </summary>
    /// <returns>Возвращает хэш-код сущности, основанный на идентификаторе и типе сущности.</returns>
    public override int GetHashCode() => HashCode.Combine(Id, GetType());

    /// <summary>
    /// Оператор равенства для сравнения двух сущностей по их идентификаторам.
    /// </summary>
    /// <param name="left">Первая сущность для сравнения</param>
    /// <param name="right">Вторая сущность для сравнения</param>
    /// <returns>Возвращает <c>true</c>, если обе сущности равны по идентификатору.</returns>
    public static bool operator ==(Entity? left, Entity? right) {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    /// <summary>
    /// Оператор неравенства для сравнения двух сущностей по их идентификаторам.
    /// </summary>
    /// <param name="left">Первая сущность для сравнения</param>
    /// <param name="right">Вторая сущность для сравнения</param>
    /// <returns>Возвращает <c>true</c>, если сущности не равны по идентификатору.</returns>
    public static bool operator !=(Entity? left, Entity? right) => !(left == right);

}

namespace dNetAPI.Models.Base;

/// <summary>
/// Интерфейс, который должны реализовывать все сущности в приложении.
/// Содержит свойства для идентификатора, даты создания и даты последнего изменения сущности.
/// </summary>
public interface IEntity {

    /// <summary>
    /// Идентификатор сущности.
    /// </summary>
    long Id { get; }

    /// <summary>
    /// Дата создания сущности.
    /// </summary>
    DateTime CreationDate { get; }

    /// <summary>
    /// Дата последнего изменения сущности. Может быть <c>null</c>, если сущность еще не была изменена.
    /// </summary>
    DateTime? ChangeDate { get; set; }
}

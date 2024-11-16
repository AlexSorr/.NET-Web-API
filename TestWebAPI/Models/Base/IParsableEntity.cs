namespace API.Models.Base;

/// <summary>
/// Интерфейс для сущностей, поддерживающих парсинг объектов в экземпляры.
/// </summary>
/// <typeparam name="T">Тип сущности, которая реализует данный интерфейс.</typeparam>
public interface IParsableEntity<T> where T : class, IEntity {

    /// <summary>
    /// Преобразует объект в экземпляр типа <typeparamref name="T"/>.
    /// </summary>
    /// <param name="object">Объект для преобразования.</param>
    /// <returns>Экземпляр типа <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidCastException">Выбрасывается, если объект не может быть преобразован в <typeparamref name="T"/>.</exception>
    static abstract T Parse(object? @object);

    /// <summary>
    /// Пытается преобразовать объект в экземпляр типа <typeparamref name="T"/>.
    /// </summary>
    /// <param name="object">Объект для преобразования.</param>
    /// <param name="result">Результат преобразования, если оно прошло успешно; иначе <c>null</c>.</param>
    /// <returns>Возвращает <c>true</c>, если преобразование было успешным; иначе <c>false</c>.</returns>
    static abstract bool TryParse(object? @object, out T? result);
    
}

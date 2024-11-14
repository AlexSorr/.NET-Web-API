using IEntity = API.Models.Base.IEntity;

namespace API.Services.Base;

/// <summary>
/// Фабрика для создания экземпляров сервисов, реализующих <see cref="IEntityService{T}"/>.
/// </summary>
public interface IEntityServiceFactory {
    /// <summary>
    /// Создает экземпляр <see cref="IEntityService{T}"/> для заданного типа сущности.
    /// </summary>
    /// <typeparam name="T">Тип сущности, для которого создается сервис. Должен реализовывать <see cref="IEntity"/>.</typeparam>
    /// <returns>Экземпляр <see cref="IEntityService{T}"/> для работы с указанным типом сущности.</returns>
    public IEntityService<T> Create<T>() where T : class, IEntity;
    
}

using IEntity = API.Models.Base.IEntity;
    
namespace API.Services.Base;

/// <summary>
/// Фабрика для создания экземпляров <see cref="IEntityService{T}"/> с использованием DI-контейнера.
/// </summary>
public class EntityServiceFactory : IEntityServiceFactory {
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="EntityServiceFactory"/> с указанным провайдером сервисов.
    /// </summary>
    /// <param name="serviceProvider">Провайдер сервисов, предоставляющий доступ к DI-контейнеру.</param>
    public EntityServiceFactory(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Создает экземпляр <see cref="IEntityService{T}"/> для заданного типа сущности.
    /// </summary>
    /// <typeparam name="T">Тип сущности, для которого создается сервис. Должен реализовывать <see cref="IEntity"/>.</typeparam>
    /// <returns>Экземпляр <see cref="IEntityService{T}"/> для работы с указанным типом сущности.</returns>
    public IEntityService<T> Create<T>() where T : class, IEntity {
        return _serviceProvider.GetService<IEntityService<T>>();
    }
}

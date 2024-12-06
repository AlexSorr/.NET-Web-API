using IEntity = dNetAPI.Models.Base.IEntity;
    
namespace dNetAPI.Services.Base;

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

    /// <inheritdoc/>
    public IEntityService<T> Create<T>() where T : class, IEntity {
        using (var scope = _serviceProvider.CreateScope()) 
            return _serviceProvider.GetService<IEntityService<T>>();
    }

}

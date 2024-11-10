using Event = API.Models.Event;

namespace API.Services;

/// <summary>
/// Интерфейс, определяющий методы для работы с сущностями типа <see cref="Event"/>.
/// Наследует функциональность от <see cref="IEntityService{Event}"/>.
/// </summary>
public interface IEventService : IEntityService<Event> {

    /// <summary>
    /// Создает новое мероприятие с заданными параметрами и генерирует соответствующие билеты.
    /// </summary>
    /// <param name="eventName">Название мероприятия.</param>
    /// <param name="eventDate">Дата мероприятия.</param>
    /// <param name="locationId">Идентификатор локации, где будет проходить мероприятие.</param>
    /// <param name="ticketsNumber">Количество билетов, которое необходимо создать для мероприятия.</param>
    /// <returns>Задача, которая возвращает созданное мероприятие.</returns>
    Task<Event> CreateEventAsync(string eventName, DateTime eventDate, long locationId, int ticketsNumber);

}

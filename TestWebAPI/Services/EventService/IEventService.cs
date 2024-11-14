using System.Linq.Expressions;
using API.Services.Base;
using Event = API.Models.Event;

namespace API.Services.EventService;

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

    /// <summary>
    /// Получить все мероприятия
    /// </summary>
    /// <returns>Полный список событий</returns>
    Task<List<Event>> GetEventsAsync();

    /// <summary>
    /// Получить все мероприятия с доп. параметрами для "жадной загрузки"
    /// </summary>
    /// <param name="eagerLoadingParameters">
    ///     Выражения с теми свойствами, которые необходимо выгрузить из БД вместе с объектом
    ///     Для Event например event => event.Tickets осуществит загрузку события со списком билетов к нему
    /// </param>
    /// <returns>Полный список событий с заполненными выбранными свойствами ссылочного типа</returns>
    Task<List<Event>> GetEventsAsync(params Expression<Func<Event, object>>[] eagerLoadingParameters); 

    /// <summary>
    /// Поиск события по id
    /// </summary>
    /// <param name="id">event_id</param>
    Task<Event> GetEventAsync(long id);

    /// <summary>
    /// Получить количество доступных билетов на мероприятие
    /// </summary>
    /// <param name="eventId">Id мероприятия</param>
    /// <returns></returns>
    Task<int> GetEventAvailableTicketCount(long eventId);

    /// <summary>
    /// Получить все доступные события 
    /// </summary>
    /// <returns></returns>
    Task<List<Event>> GetAvailableEvents();

    /// <summary>
    /// Получить список событий на определенную дату 
    /// </summary>
    /// <param name="date">Дата мероприятия</param>
    /// <param name="onlyAvailable">Только с доступными к покупке билетами</param>
    /// <returns></returns>
    Task<List<Event>> GetEventsByDate(DateTime date, bool onlyAvailable = false);


}

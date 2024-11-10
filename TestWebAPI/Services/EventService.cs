using System.Linq.Expressions;
using System.Text;
using API.Models;

namespace API.Services;

/// <summary>
/// Сервис для работы с событиями
/// </summary>
public class EventService : EntityService<Event>, IEventService {

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="EventService"/> с указанными параметрами.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    /// <param name="logger">Логгер для записи событий.</param>
    public EventService(ApplicationDbContext context, ILogger<EntityService<Event>> logger) : base(context, logger) { }
    
    /// <summary>
    /// Создает новое событие с указанными параметрами.
    /// </summary>
    /// <param name="eventName">Название события.</param>
    /// <param name="eventDate">Дата события.</param>
    /// <param name="locationId">Идентификатор места проведения.</param>
    /// <param name="ticketsNumber">Количество билетов.</param>
    /// <returns>Созданное событие.</returns>
    /// <exception cref="Exception">Выбрасывается при неверных данных события.</exception>
    public async Task<Event> CreateEventAsync(string eventName, DateTime eventDate, long locationId, int ticketsNumber) {
        StringBuilder errors = new StringBuilder(); 
        if (string.IsNullOrWhiteSpace(eventName))
            errors.Append("Некорректное название события.");
        if (eventDate <= DateTime.Now)
            errors.Append("Дата события не может быть меньше текущей.");

        Location location;
        if (!LocationExists(locationId, out location))
            errors.Append($"Локация с идентификатором {locationId} не найдена.");
        
        if (ticketsNumber <= 0)
            errors.Append($"Некорректное количество билетов: {ticketsNumber}.");
        
        if (!string.IsNullOrEmpty(errors.ToString()))
            throw new Exception(errors.ToString());

        eventDate = DateTime.SpecifyKind(eventDate, DateTimeKind.Utc);

        Event result = new Event() { Name = eventName, Date = eventDate, Location = location };
        CreateTicketsForEvent(result, ticketsNumber);

        await SaveAsync(result);

        return result;
    }

    /// <summary>
    /// Получить все мероприятия
    /// </summary>
    /// <returns>Полный список событий</returns>
    public async Task<List<Event>> GetEventsAsync() {
        return await GetAllAsync();
    }

    /// <summary>
    /// Получить все мероприятия с доп. параметрами для "жадной загрузки"
    /// </summary>
    /// <param name="eagerLoadingParameters">
    ///     Выражения с теми свойствами, которые необходимо выгрузить из БД вместе с объектом
    ///     Для Event например event => event.Tickets осуществит загрузку события со списком билетов к нему
    /// </param>
    /// <returns>Полный список событий с заполненными выбранными свойствами ссылочного типа</returns>
    public async Task<List<Event>> GetEventsAsync(params Expression<Func<Event, object>>[] eagerLoadingParameters) {
        return await GetAllWithRelatedDataAsync(eagerLoadingParameters);
    } 

    /// <summary>
    /// Поиск события по id
    /// </summary>
    /// <param name="id">event_id</param>
    public async Task<Event> GetEventAsync(long id) {
        return await LoadByIdAsync(id);
    }

    /// <summary>
    /// Получить количество доступных билетов на мероприятие
    /// </summary>
    /// <param name="eventId">Id мероприятия</param>
    /// <returns></returns>
    public async Task<int> GetEventAvailableTicketCount(long eventId) {
        Event @event = await LoadByIdWithRelatedDataAsync(eventId, ev => ev.Tickets);
        if (@event == null) {
            _logger.LogError($"При запросе количества билетов не найдено событие по Id {eventId}");
            return 0;
        }
        return @event.AvailableTickets.Count();
    }

    /// <summary>
    /// Получить все доступные события 
    /// </summary>
    /// <returns></returns>
    public async Task<List<Event>> GetAvailableEvents() {
        List<Event> events = await GetAllAsync(x => x.Date >= DateTime.UtcNow && x.AvailableTickets.Any());
        return events;
    }

    /// <summary>
    /// Получить список событий на определенную дату 
    /// </summary>
    /// <param name="date">Дата мероприятия</param>
    /// <param name="onlyAvailable">Только с доступными к покупке билетами</param>
    /// <returns></returns>
    public async Task<List<Event>> GetEventsByDate(DateTime date, bool onlyAvailable = false) {
        List<Event> events = await GetAllAsync(ev => ev.Date == date);
        if (events == null || !events.Any()) return new List<Event>();
        return onlyAvailable ? events.Where(e => e.AvailableTickets.Any()).ToList() : events;
    }

    #region private_methods

    /// <summary>
    /// Создает билеты для указанного события.
    /// </summary>
    /// <param name="createdEvent">Событие, для которого создаются билеты.</param>
    /// <param name="ticketsCount">Количество билетов.</param>
    private void CreateTicketsForEvent(Event createdEvent, int ticketsCount) {
        if (createdEvent == null || ticketsCount <= 0) return;

        for (int i = 1; i <= ticketsCount; i++) {
            Ticket res = new Ticket() {
                Number = GenerateTicketNumber(i, ticketsCount),
                Event = createdEvent,
                BookingStatus = BookingStatus.Free
            };
            createdEvent.Tickets.Add(res);
        }
    }
    
    /// <summary>
    /// Генерирует номер билета в формате с лидирующими нулями.
    /// </summary>
    /// <param name="number">Номер билета.</param>
    /// <param name="ticketsCount">Общее количество билетов.</param>
    /// <returns>Сформированный номер билета.</returns>
    private string GenerateTicketNumber(int number, int ticketsCount) {
        int maxCharLength = ticketsCount.ToString().Length;
        int numberCharLength = number.ToString().Length;
        int zerosCount = maxCharLength - numberCharLength;
        return $"{new string('0', zerosCount)}{number}";
    }

    /// <summary>
    /// Проверяет, существует ли локация с указанным идентификатором.
    /// </summary>
    /// <param name="locationId">Идентификатор локации.</param>
    /// <param name="location">Локация, если найдена.</param>
    /// <returns>True, если локация найдена; иначе false.</returns>
    private bool LocationExists(long locationId, out Location location) {
        location = _context.Set<Location>().Find(locationId);
        return location != null;
    }

    #endregion
}

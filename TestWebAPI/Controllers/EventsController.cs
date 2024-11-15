using API.Controllers.Base;
using API.Models;
using Microsoft.AspNetCore.Mvc;

using API.Services.EventService;

/// <summary>
/// Контроллер для обработки событий
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EventsController : APIBaseController<Event> {

    private readonly IEventService _eventService;
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="EventsController"/> с указанными параметрами.
    /// </summary>
    /// <param name="context">Котекст БД</param>
    /// <param name="logger">Логгер</param>
    /// <param name="entityService">Сервис для работы с сущностями</param>
    public EventsController(ApplicationDbContext context, ILogger<EventsController> logger, IEventService entityService): base(context, logger, entityService) { 
        _eventService = entityService;
    }

    /// <summary>
    /// Создание нового события
    /// </summary>
    /// <param name="eventName">Название события</param>
    /// <param name="eventDate">Дата события</param>
    /// <param name="locationId">Id Места проведения события</param>
    /// <param name="ticketsNumber">Количество билетов, которые будут сгенерены для события</param>
    /// <returns>Id созданного события</returns>
    [HttpPost("create_event")]
    public async Task<ActionResult<long>> CreateEvent(string eventName, DateTime eventDate, long locationId, int ticketsNumber) {
        try {
            return Ok(await _eventService.CreateEventAsync(eventName, eventDate, locationId, ticketsNumber));
        } catch (Exception ex) {
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Получить все мероприятия
    /// </summary>
    /// <returns>event_list</returns>
    [HttpGet("get_all_events")]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents() {
        List<Event> events = await _entityService.GetAllWithRelatedDataAsync(y => y.Location);
        return events != null && events.Any() ? Ok(events) : NotFound("Events not found");
    } 

    /// <summary>
    /// Поиск события по id
    /// </summary>
    /// <param name="id">event_id</param>
    [HttpGet("get_event_{id}")]
    public async Task<ActionResult<Event>> GetEvent(long id) {
        Event eventData = await _entityService.LoadByIdWithRelatedDataAsync(id, x => x.Tickets, y => y.Location);
        return eventData == null ? NotFound($"Event with id {id} not exists") : Ok(eventData);
    }

    /// <summary>
    /// Получить количество доступных билетов на мероприятие
    /// </summary>
    /// <param name="eventId">Id мероприятия</param>
    /// <returns></returns>
    [HttpGet("get_event_available_tickets_count")]
    public async Task<ActionResult<int>> GetEventAvailableTicketCount(long eventId) {
        Event @event = await _entityService.LoadByIdWithRelatedDataAsync(eventId, e => e.Tickets);
        return @event == null ? NotFound($"Event with id {eventId} not found!") : Ok(@event.AvailableTickets.Count());
    }

    /// <summary>
    /// Удаление события по id
    /// </summary>
    /// <param name="id">event_id</param>
    [HttpDelete("delete_event_{id}")]
    public async Task<IActionResult> DeleteEvent(long id) {
        try {
            await _entityService.DeleteAsync(id);
        } catch (Exception ex) {
            return HandleError(ex);
        }
        return NoContent();
    }

}
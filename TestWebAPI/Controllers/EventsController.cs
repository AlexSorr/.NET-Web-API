using API.Controllers.Base;
using API.Models;
using Microsoft.AspNetCore.Mvc;

using API.Services;


[Route("api/[controller]")]
[ApiController]
public class EventsController : APIBaseController {

    protected readonly IEventService _entityService;

    public EventsController(ApplicationDbContext context, ILogger<EventsController> logger, IEventService entityService): base(context, logger) { 
        _entityService = entityService;
    }

    // Создание события
    [HttpPost("create_event")]
    public async Task<ActionResult<Event>> CreateEvent(string eventName, DateTime eventDate, long locationId, int ticketsNumber) {
        try {
            return Ok(await _entityService.CreateEventAsync(eventName, eventDate, locationId, ticketsNumber));
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
        IEnumerable<Event> events = await _entityService.GetAllAsync();
        return events != null && events.Any() ? Ok(events) : NotFound("Events not found");
    } 


    /// <summary>
    /// Поиск события по id
    /// </summary>
    /// <param name="id">event_id</param>
    [HttpGet("get_event_{id}")]
    public async Task<ActionResult<Event>> GetEvent(long id) {
        Event eventData = await _entityService.LoadByIdAsync(id);
        return eventData == null ? NotFound($"Event with id {id} not exists") : Ok(eventData);
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
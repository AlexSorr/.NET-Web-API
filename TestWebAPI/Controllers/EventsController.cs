using API.Controllers.Base;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class EventsController : APIBaseController {

    public EventsController(ApplicationDbContext context, ILogger<EventsController> logger): base(context, logger) { }

    // Создание события
    [HttpPost("create_event")]
    public async Task<ActionResult<Event>> CreateEvent(Event eventData) {
        _context.Events.Add(eventData);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEvent), new { id = eventData.Id }, eventData);
    }

    /// <summary>
    /// Получить все мероприятия
    /// </summary>
    /// <returns>event_list</returns>
    [HttpGet("get_all_events")]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents() => await _context.Events.ToListAsync();


    /// <summary>
    /// Поиск события по id
    /// </summary>
    /// <param name="id">event_id</param>
    [HttpGet("get_event_{id}")]
    public async Task<ActionResult<Event>> GetEvent(long id) {
        Event? eventData = await _context.Events.FindAsync(id);
        return eventData == null ? NotFound($"Event with id {id} not exists") : Ok(eventData);
    }


    /// <summary>
    /// Удаление события по id
    /// </summary>
    /// <param name="id">event_id</param>
    [HttpDelete("delete_event_{id}")]
    public async Task<IActionResult> DeleteEvent(long id) {
        Event? eventData = await _context.Events.FindAsync(id);
        if (eventData == null)
            return NotFound();

        try {
            _context.Events.Remove(eventData);
            await _context.SaveChangesAsync();
        } catch (Exception ex) {
            return HandleError(ex);
        }

        return NoContent();
    }

}
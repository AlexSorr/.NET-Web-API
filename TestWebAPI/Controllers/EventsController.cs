using API.Controllers.Base;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class EventsController : APIBaseController {

    public EventsController(ApplicationDbContext context, ILogger<EventsController> logger): base(context, logger) { }

    // Создание события
    [HttpPost]
    public async Task<ActionResult<Event>> CreateEvent(Event eventData) {
        _context.Events.Add(eventData);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEvent), new { id = eventData.Id }, eventData);
    }

    /// <summary>
    /// Получить все мероприятия
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents() => await _context.Events.ToListAsync();


    /// <summary>
    /// Поиск события по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEvent(long id) {
        Event? eventData = await _context.Events.FindAsync(id);
        return eventData == null ? NotFound($"Event with id {id} not exists") : Ok(eventData);
    }

    // Удаление события
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(long id) {
        Event? eventData = await _context.Events.FindAsync(id);
        if (eventData == null)
            return NotFound();
        
        _context.Events.Remove(eventData);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Обновление события
    // [HttpPut("{id}")]
    // public async Task<IActionResult> UpdateEvent(long id, Event eventData) {

    //     if (!EventExists(id))
    //         return NotFound($"Event id: {id} not found!");

    //     if (id != eventData.Id) 
    //         return BadRequest($"Can't match event {id} with event {eventData.Id}");
        
    //     _context.Entry(eventData).State = EntityState.Modified;

    //     try {
    //         await _context.SaveChangesAsync();
    //     } catch (DbUpdateConcurrencyException ex) {
    //         _logger.LogError(ex.ToString());
    //     }

    //     return NoContent();
    // }

}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Models;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase {

    private readonly ApplicationDbContext _context;

    private readonly ILogger<EventsController> _logger;

    public EventsController(ApplicationDbContext context, ILogger<EventsController> logger) { 
        _context = context; 
        _logger = logger;
    }

    // Создание события
    [HttpPost]
    public async Task<ActionResult<Event>> CreateEvent(Event eventData) {
        _context.Events.Add(eventData);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEvent), new { id = eventData.Id }, eventData);
    }

    // Чтение всех событий
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents() => await _context.Events.ToListAsync();


    // Чтение одного события по Id
    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEvent(long id) {
        Event? eventData = await _context.Events.FindAsync(id);
        return eventData == null ? NotFound() : Ok(eventData);
    }


    // Обновление события
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(long id, Event eventData) {

        if (!EventExists(id))
            return NotFound($"Event id: {id} not found!");

        if (id != eventData.Id) 
            return BadRequest($"Can't match event {id} with event {eventData.Id}");
        
        _context.Entry(eventData).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException ex) {
            _logger.LogError(ex.ToString());
        }

        return NoContent();
    }


    // Удаление события
    //работакт
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(long id) {
        Event? eventData = await _context.Events.FindAsync(id);
        if (eventData == null)
            return NotFound();
        
        _context.Events.Remove(eventData);
        await _context.SaveChangesAsync();

        return NoContent();
    }


    private bool EventExists(long id) => _context.Events.Any(e => e.Id == id);

}
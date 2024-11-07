using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Models;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public EventsController(ApplicationDbContext context) { _context = context; }

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
        var eventData = await _context.Events.FindAsync(id);
        if (eventData == null)
            return NotFound();
        return eventData == null ? NotFound() : eventData;
    }

    // Обновление события
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(long id, Event eventData) {
        if (!EventExists(id))
            return NotFound();

        if (id != eventData.Id) 
            return BadRequest();
        
        _context.Entry(eventData).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException ex) {
            Console.WriteLine(ex + string.Empty);
        }

        return NoContent();
    }

    // Удаление события
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(long id) {
        var eventData = await _context.Events.FindAsync(id);
        if (eventData == null)
            return NotFound();
        
        _context.Events.Remove(eventData);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EventExists(long id) => _context.Events.Any(e => e.Id == id);

}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using API.Models;
using API.Controllers.Base;


[Route("api/[controller]")]
[ApiController]
public class BookingsController : APIBaseController {

    public BookingsController(ApplicationDbContext context, ILogger<BookingsController> logger) : base(context, logger) { } 

    // GET: api/Bookings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings() => await _context.Bookings.Include(b => b.Event).ToListAsync();

    // GET: api/Bookings/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetBooking(long id) {
        Booking? booking = await _context.Bookings.Include(b => b.Event).FirstOrDefaultAsync(b => b.Id == id);
        return booking == null ? NotFound() : Ok(booking);
    }

    // POST: api/Bookings
    [HttpPost]
    public async Task<ActionResult<Booking>> PostBooking(Booking booking) {
        // Проверка на доступность билетов
        if (booking.Event.AvailableTickets < booking.NumberOfTickets) 
            return BadRequest("Not enough available tickets.");
        
        booking.Event.AvailableTickets -= booking.NumberOfTickets;
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
    }

    // PUT: api/Bookings/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBooking(long id, Booking booking) {

        if (!_context.Bookings.Any(b => b.Id == id)) 
            return NotFound();

        if (id != booking.Id) {
            _logger.LogError($"Requested Id: {id}; sended booking id: {booking?.Id}");
            return BadRequest();
        }
            
        
        _context.Entry(booking).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException ex) {
            _logger.LogError(ex.ToString());
        }

        return NoContent();
    }

    // DELETE: api/Bookings/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(long id) {
        var booking = await _context.Bookings.FindAsync(id);
        
        if (booking == null)
            return NotFound();
        
        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();

        // Возврат количества билетов в Event
        var eventItem = await _context.Events.FindAsync(booking.Event.Id);
        if (eventItem != null) {
            eventItem.AvailableTickets += booking.NumberOfTickets;
            await _context.SaveChangesAsync();
        }

        return NoContent();
    }

}

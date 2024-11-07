using Microsoft.AspNetCore.Mvc;
using Models;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public BookingsController(ApplicationDbContext context) { _context = context; }

    // Бронирование билетов
    [HttpPost]
    public async Task<ActionResult<Booking>> CreateBooking(Booking bookingData) {
        var eventData = await _context.Events.FindAsync(bookingData.EventId);
        if (eventData == null) 
            return NotFound("Event not found.");

        if (eventData.AvailableTickets < bookingData.NumberOfTickets)
            return BadRequest("Not enough available tickets.");

        eventData.AvailableTickets -= bookingData.NumberOfTickets;
        _context.Bookings.Add(bookingData);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBooking), new { id = bookingData.Id }, bookingData);
    }

    // Чтение бронирования по Id
    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetBooking(long id) {
        var booking = await _context.Bookings.FindAsync(id);
        return booking == null ? NotFound() : booking;
    }
}
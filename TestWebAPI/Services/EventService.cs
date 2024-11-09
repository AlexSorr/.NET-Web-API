using System.Text;
using API.Models;

namespace API.Services;

public class EventService : EntityService<Event>, IEventService {

    public EventService(ApplicationDbContext context, ILogger<EntityService<Event>> logger) : base(context, logger) { }
    
    public async Task<Event> CreateEventAsync(string eventName, DateTime eventDate, long locationId, int ticketsNumber) {
        StringBuilder errors = new StringBuilder(); 
        if (string.IsNullOrWhiteSpace(eventName))
            errors.Append("Incorrect event name");
        if (eventDate <= DateTime.Now)
            errors.Append("Start date is lower than current date");

        Location location;
        if (!LocationExists(locationId, out location))
            errors.Append($"Location with id {locationId} not found");
        
        if (ticketsNumber <= 0)
            errors.Append($"Incorrect tickets number: {ticketsNumber}");
        
        if (!string.IsNullOrEmpty(errors.ToString()))
            throw new Exception(errors.ToString());

        // Преобразование eventDate в UTC для postgre
        eventDate = DateTime.SpecifyKind(eventDate, DateTimeKind.Utc);

        Event result = new Event() { Name = eventName, Date = eventDate, Location = location };
        CreateTicketsForEvent(result, ticketsNumber);

        await SaveAsync(result);

        return result;
    }

    /// <summary>
    /// Создать билеты к мероприятию
    /// </summary>
    /// <param name="createdEvent"></param>
    /// <param name="ticketsCount"></param>
    /// <returns></returns>
    public void CreateTicketsForEvent(Event createdEvent, int ticketsCount) {
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
    /// Генерируем номер билета
    /// 00001, 0002
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private string GenerateTicketNumber(int number, int ticketsCount) {
        int max_char_length = (ticketsCount + string.Empty).Length;
        int number_char_length = (number + string.Empty).Length;

        //количество нулей, которые надо добавить вначале
        int zeros_count = max_char_length - number_char_length;
        return new string('0', zeros_count) + number;
    }


    /// <summary>
    /// Проверка, что локация по id существует в базе
    /// </summary>
    /// <param name="locationId"></param>
    /// <param name="location"></param>
    /// <returns></returns>
    private bool LocationExists(long locationId, out Location location) {
        location = _context.Set<Location>().FirstOrDefault(x => x.Id == locationId);
        return location != null;
    }

}

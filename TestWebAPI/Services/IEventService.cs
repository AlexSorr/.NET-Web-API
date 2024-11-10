using Event = API.Models.Event;

namespace API.Services;

public interface IEventService : IEntityService<Event> {
    
    Task<Event> CreateEventAsync(string eventName, DateTime eventDate, long locationId, int ticketsNumber);

}

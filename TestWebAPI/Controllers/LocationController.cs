using API.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Services;

[Route("api/[controller]")]
[ApiController]
public class LocationController : APIBaseController<Location> {

    public LocationController(ApplicationDbContext context, ILogger<LocationController> logger, IEntityService<Location> entityService) : base(context, logger, entityService) { }

    /// <summary>
    /// Загрузить локации
    /// </summary>
    /// <param name="objects"></param>
    /// <returns></returns>
    [HttpPost("load_locations")]
    public async Task<IActionResult> UploadLocations([FromBody] IEnumerable<Location> objects) {
        try {
            await _entityService.SaveBatchAsync(objects);        
        } catch (Exception ex) { return HandleError(ex); }
        return Ok();
    }
    
}
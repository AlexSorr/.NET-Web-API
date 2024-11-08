
using Microsoft.AspNetCore.Mvc;
using Models;

[Route("api/[controller]")]
[ApiController]
public class LocationController : ControllerBase {
    private readonly ApplicationDbContext _context;

    private readonly ILogger<EventsController> _logger;

    private readonly DataLoader _dataLoader;

    public LocationController(ApplicationDbContext context, ILogger<EventsController> logger, DataLoader dataLoader) { 
        _context = context; 
        _logger = logger;
        _dataLoader = dataLoader;
    }

    [HttpPost("loader/from_file")]
    public async Task<IActionResult> LoadFromFileAsync([FromForm] IFormFile file) {
        try {
            await _dataLoader.UploadDataFromFileAsync<Location>(file);
            return Ok("Data uploaded");
        } catch (Exception ex) {
            _logger.LogError("Error data loading\n" + ex.ToString());
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("loader/from_request")]
    public async Task<IActionResult> UploadFromRequest([FromBody] List<Location> locations) {
        try {
            await _dataLoader.UploadDataAsync<Location>(locations);
            return Ok("Data uploaded");
        } catch (Exception ex) {
            _logger.LogError("Error data loading\n" + ex.ToString());
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
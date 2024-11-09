using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base;

[Route("api/[controller]")]
public abstract class APIBaseController : ControllerBase {

    protected readonly ApplicationDbContext _context;
    protected readonly ILogger<APIBaseController> _logger;

    protected APIBaseController(ApplicationDbContext context, ILogger<APIBaseController> logger) {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Обработка ошибки для дочерних контроллеров
    /// </summary>
    protected ActionResult HandleError(Exception ex)  {
        _logger.LogError(ex, "An error occurred");
        return StatusCode(500, $"An internal error occurred: {ex.Message}");
    }
    
}
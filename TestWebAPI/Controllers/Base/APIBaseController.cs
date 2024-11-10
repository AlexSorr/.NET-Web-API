using API.Models.Base;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base;

[Route("api/[controller]")]
public abstract class APIBaseController<T> : ControllerBase where T : IEntity {

    protected readonly ApplicationDbContext _context;
    protected readonly ILogger<APIBaseController<T>> _logger;
    protected readonly IEntityService<T> _entityService;

    protected APIBaseController(ApplicationDbContext context, ILogger<APIBaseController<T>> logger, IEntityService<T> entityService) {
        _context = context;
        _logger = logger;
        _entityService = entityService;
    }

    /// <summary>
    /// Обработка ошибки для дочерних контроллеров
    /// </summary>
    protected ActionResult HandleError(Exception ex)  {
        _logger.LogError(ex, "An error occurred");
        return StatusCode(500, $"An internal error occurred: {ex.Message}");
    }
    
}
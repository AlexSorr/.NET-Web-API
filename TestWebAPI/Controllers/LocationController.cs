using API.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Services;


/// <summary>
/// Контроллер для работы с локациями. Наследуется от <see cref="APIBaseController{T}"/> для работы с сущностями типа <see cref="Location"/>.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class LocationController : APIBaseController<Location> {

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="LocationController"/> с указанными параметрами.
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="logger">Логгер для логирования событий</param>
    /// <param name="entityService">Сервис для работы с сущностями локации</param>
    public LocationController(ApplicationDbContext context, ILogger<LocationController> logger, IEntityService<Location> entityService) : base(context, logger, entityService) { }

    /// <summary>
    /// Загружает список локаций в систему.
    /// </summary>
    /// <param name="objects">Список объектов локаций для сохранения</param>
    /// <returns>Ответ с кодом состояния 200, если локации успешно загружены</returns>
    [HttpPost("load_locations")]
    public async Task<IActionResult> UploadLocations([FromBody] IEnumerable<Location> objects) {
        try {
            // Сохраняем локации в базе данных
            await _entityService.SaveBatchAsync(objects);        
        } catch (Exception ex) { 
            // Обработка ошибок, если они возникнут
            return HandleError(ex); 
        }
        return Ok(); // Возвращаем успешный статус
    }
}

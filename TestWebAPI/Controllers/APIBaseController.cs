using dNetAPI.Models.Base;
using dNetAPI.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace dNetAPI.Controllers.Base;

/// <summary>
/// Базовый контроллер для работы с сущностями типа <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Тип сущности, с которой работает контроллер. Должен реализовывать интерфейс <see cref="IEntity"/>.</typeparam>
[Route("api/[controller]")]
public abstract class APIBaseController<T> : ControllerBase where T : IEntity {

    /// <summary>
    /// Логгер для логирования ошибок и других событий в контроллере.
    /// </summary>
    protected readonly ILogger<APIBaseController<T>> _logger;

    /// <summary>
    /// Сервис для работы с сущностями типа <typeparamref name="T"/>.
    /// </summary>
    protected readonly IEntityService<T> _entityService;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="APIBaseController{T}"/> с указанными параметрами.
    /// </summary>
    /// <param name="logger">Логгер для логирования событий</param>
    /// <param name="entityService">Сервис для работы с сущностями</param>
    protected APIBaseController(ILogger<APIBaseController<T>> logger, IEntityService<T> entityService) {
        _logger = logger;
        _entityService = entityService;
    }

    #region Get

    /// <summary>
    /// Поиск по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Id</returns>
    [HttpGet("get_id_{id}")]
    public async Task<ActionResult<long>> GetById(long id) => NotFound();
    
    #endregion

    #region Post

    /// <summary>
    /// Создание новой сущности
    /// </summary>
    /// <returns></returns>
    [HttpPost("create_new")]
    public async Task<ActionResult> Create() => NoContent();

    #endregion

    #region Delete

    /// <summary>
    /// Удаление по id
    /// </summary>
    /// <param name="id">Id</param>
    [HttpDelete("delete_id_{id}")]
    public async Task<IActionResult> Delete(long id) => NoContent();

    
    #endregion

    #region Put

    [HttpPut("put_id_{id}")]
    public async Task<ActionResult> Put() => NoContent();

    #endregion

    /// <summary>
    /// Обрабатывает ошибки, возникающие в дочерних контроллерах, и возвращает соответствующий ответ.
    /// </summary>
    /// <param name="ex">Исключение, возникшее во время работы</param>
    /// <returns>Ответ с кодом состояния 500 и сообщением об ошибке</returns>
    protected ActionResult HandleError(Exception ex)  {
        _logger.LogError(ex, "An error occurred");
        return StatusCode(500, $"An internal error occurred: {ex.Message}");
    }
}

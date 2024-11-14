using API.Models.Base;
using API.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base;

/// <summary>
/// Базовый контроллер для работы с сущностями. 
/// Все контроллеры, работающие с сущностями, должны наследоваться от этого класса.
/// </summary>
[Route("api/[controller]")]
public abstract class APIBaseController<T> : ControllerBase where T : IEntity {

    /// <summary>
    /// Контекст базы данных для доступа к данным.
    /// </summary>
    protected readonly ApplicationDbContext _context;

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
    /// <param name="context">Контекст базы данных</param>
    /// <param name="logger">Логгер для логирования событий</param>
    /// <param name="entityService">Сервис для работы с сущностями</param>
    protected APIBaseController(ApplicationDbContext context, ILogger<APIBaseController<T>> logger, IEntityService<T> entityService) {
        _context = context;
        _logger = logger;
        _entityService = entityService;
    }

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

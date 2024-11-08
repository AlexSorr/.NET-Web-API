using Microsoft.AspNetCore.Mvc;
using API.Data;


namespace API.Controllers.Base;

/// <summary>
/// Контроллер для сущностей, которые массово загружаются
/// Справочники, все дела
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public abstract class BulkUploadController<T> : APIBaseController, IBulkUploadController<T> where T : class {

    protected DataLoader _dataLoader;

    protected BulkUploadController(ApplicationDbContext context, ILogger<APIBaseController> logger, DataLoader dataLoader) : base(context, logger ) {
        this._dataLoader = dataLoader;
    }

    /// <summary>
    /// Заливка сущностей из файла
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("load/from_file")]
    public async Task<IActionResult> UploadFromFile([FromForm] IFormFile file) {
        try {
            await _dataLoader.UploadDataFromFileAsync<T>(file);
            return Ok("Data uploaded");
        } catch (Exception ex) {
            _logger.LogError(ex.ToString());
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Добавление сущностей по реквесту
    /// </summary>
    /// <param name="locations"></param>
    /// <returns></returns>
    [HttpPost("load/from_request")]
    public async Task<IActionResult> UploadFromRequest([FromBody] IEnumerable<T> objects) {
        try {
            await _dataLoader.UploadDataAsync<T>(objects);
            return Ok("Data uploaded");
        } catch (Exception ex) {
            _logger.LogError("Error data loading\n" + ex.ToString());
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
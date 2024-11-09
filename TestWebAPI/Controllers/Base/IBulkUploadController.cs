using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base;

public interface IBulkUploadController<T> where T : class {

    //Task<IActionResult> UploadFromFile([FromForm] IFormFile file);

    Task<IActionResult> UploadFromRequest(IEnumerable<T> objects);

}

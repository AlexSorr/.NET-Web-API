

using API.Models.Base;
using Microsoft.AspNetCore.Mvc;

namespace API.Services;

public interface IEntityService<T> where T : IEntity {

    Task<T> GetEntityAsync(long id);

    Task<ActionResult<IEnumerable<T>>> GetAllAsync();

    Task DeleteAsync(long id);

    bool EntityExists(long id);

}

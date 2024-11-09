

using API.Models.Base;
using Microsoft.AspNetCore.Mvc;

namespace API.Services;

public interface IService<T> where T : IEntity {

    Task<T> GetEntityAsync(long id);

    Task<IEnumerable<T>> GetAllAsync();

    Task DeleteAsync(long id);

    bool EntityExists(long id);

}

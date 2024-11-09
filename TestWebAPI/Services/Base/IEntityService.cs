

using API.Models.Base;
using Microsoft.AspNetCore.Mvc;

namespace API.Services;

public interface IEntityService<T> where T : IEntity {

    /// <summary>
    /// Создать сущность (сохранить в БД)
    /// </summary>
    /// <returns></returns>
    public Task SaveAsync(T entity);

    /// <summary>
    /// Пакетное сохранение списка сущностей
    /// </summary>
    /// <param name="batch"></param>
    /// <param name="bathcSize"></param>
    /// <returns></returns>
    public Task SaveBatchAsync(IEnumerable<T> batch, int bathcSize = 1000);

    /// <summary>
    /// Поиск по id объекта
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<T> LoadByIdAsync(long id);

    /// <summary>
    /// Получить все сущности
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<T>> GetAllAsync();

    public Task DeleteRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Удалить асинхронно
    /// </summary>
    /// <param name="entity">сущность</param>
    /// <returns></returns>
    public Task DeleteAsync(T entity);

    /// <summary>
    /// Удалить асинхронно
    /// </summary>
    /// <param name="id">id сущности</param>
    /// <returns></returns>
    public Task DeleteAsync(long id);

    
    /// <summary>
    /// Сущность есть в БД
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool EntityExists(long id);


    /// <summary>
    /// Сущность есть в БД, возвращает результат
    /// </summary>
    /// <param name="id"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool EntityExists(long id, out T result);

}

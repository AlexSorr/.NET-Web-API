using System.Linq.Expressions;
using API.Models.Base;

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
    public Task SaveBatchAsync(IEnumerable<T> batch);


    /// <summary>
    /// Поиск по id объекта
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<T> LoadByIdAsync(long id);


    /// <summary>
    /// Загрузить с доп. данными
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    public Task<T> LoadByIdWithRelatedDataAsync(long id, params Expression<Func<T, object>>[] includes);


    /// <summary>
    /// Получить все сущности
    /// </summary>
    /// <returns></returns>
    public Task<List<T>> GetAllAsync();


    /// <summary>
    /// Получить все сущности с доп. данными
    /// </summary>
    /// <param name="includes"></param>
    /// <returns></returns>
    Task<List<T>> GetAllWithRelatedDataAsync(params Expression<Func<T, object>>[] includes);


    /// <summary>
    /// Удалить множество асинхронно
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
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

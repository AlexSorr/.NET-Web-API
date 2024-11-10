using System.Linq.Expressions;
using API.Models.Base;

namespace API.Services;

/// <summary>
/// Интерфейс, определяющий методы для работы с сущностями в базе данных.
/// </summary>
/// <typeparam name="T">Тип сущности, с которой будет работать сервис. Должен реализовывать интерфейс <see cref="IEntity"/>.</typeparam>
public interface IEntityService<T> where T : IEntity {

    /// <summary>
    /// Создает или обновляет сущность и сохраняет её в базе данных.
    /// </summary>
    /// <param name="entity">Сущность для сохранения или обновления в базе данных.</param>
    /// <returns>Задача, представляющая асинхронную операцию сохранения.</returns>
    public Task SaveAsync(T entity);

    /// <summary>
    /// Пакетно сохраняет список сущностей в базе данных.
    /// </summary>
    /// <param name="batch">Коллекция сущностей для пакетного сохранения.</param>
    /// <returns>Задача, представляющая асинхронную операцию пакетного сохранения.</returns>
    public Task SaveBatchAsync(IEnumerable<T> batch);

    /// <summary>
    /// Загружает сущность по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Задача, которая возвращает найденную сущность или null, если сущность не найдена.</returns>
    public Task<T> LoadByIdAsync(long id);

    /// <summary>
    /// Загружает сущность по идентификатору с включением дополнительных связанных данных.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="includes">Выражения для включения дополнительных связанных данных в запрос.</param>
    /// <returns>Задача, которая возвращает найденную сущность с включёнными связанными данными или null, если сущность не найдена.</returns>
    public Task<T> LoadByIdWithRelatedDataAsync(long id, params Expression<Func<T, object>>[] includes);

    /// <summary>
    /// Загружает все сущности указанного типа из базы данных.
    /// </summary>
    /// <returns>Задача, которая возвращает список всех сущностей.</returns>
    public Task<List<T>> GetAllAsync();

    /// <summary>
    /// Возвращает все сущности данного типа, отфильтрованные по выражению.
    /// </summary>
    /// <param name="predicate">Выражение для фильтрации сущностей.</param>
    /// <returns>Список отфильтрованных сущностей.</returns>
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Загружает все сущности с включением дополнительных связанных данных.
    /// </summary>
    /// <param name="includes">Выражения для включения связанных данных в запрос.</param>
    /// <returns>Задача, которая возвращает список всех сущностей с включёнными связанными данными.</returns>
    public Task<List<T>> GetAllWithRelatedDataAsync(params Expression<Func<T, object>>[] includes);

    /// <summary>
    /// Удаляет несколько сущностей из базы данных асинхронно.
    /// </summary>
    /// <param name="entities">Коллекция сущностей для удаления.</param>
    /// <returns>Задача, представляющая асинхронную операцию удаления сущностей.</returns>
    public Task DeleteRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Удаляет одну сущность из базы данных асинхронно.
    /// </summary>
    /// <param name="entity">Сущность для удаления.</param>
    /// <returns>Задача, представляющая асинхронную операцию удаления сущности.</returns>
    public Task DeleteAsync(T entity);

    /// <summary>
    /// Удаляет сущность по её идентификатору из базы данных асинхронно.
    /// </summary>
    /// <param name="id">Идентификатор сущности для удаления.</param>
    /// <returns>Задача, представляющая асинхронную операцию удаления сущности по идентификатору.</returns>
    public Task DeleteAsync(long id);

    /// <summary>
    /// Проверяет, существует ли сущность с заданным идентификатором в базе данных.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Возвращает true, если сущность существует в базе данных, иначе false.</returns>
    public bool EntityExists(long id);

    /// <summary>
    /// Проверяет существование сущности по идентификатору и возвращает саму сущность.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="result">Сущность, найденная по идентификатору (если она существует).</param>
    /// <returns>Возвращает true, если сущность существует, иначе false.</returns>
    public bool EntityExists(long id, out T result);
}

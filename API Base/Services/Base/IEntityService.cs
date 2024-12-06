using System.Linq.Expressions;
using dNetAPI.Models.Base;

namespace dNetAPI.Services.Base;

/// <summary>
/// Интерфейс, определяющий методы для работы с сущностями в базе данных.
/// </summary>
/// <typeparam name="T">Тип сущности, с которой будет работать сервис. Должен реализовывать интерфейс <see cref="IEntity"/>.</typeparam>
public interface IEntityService<T> where T : IEntity {

    /// <summary>
    /// Получить сущность <typeparamref name="T"/> по Id
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Найденная сущность или <c>null</c></returns>
    public T LoadById(long id);

    /// <summary>
    /// Загружает сущность по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Задача, которая возвращает найденную сущность или <c>null</c>, если сущность не найдена.</returns>
    public Task<T> LoadByIdAsync(long id);

    /// <summary>
    /// Найти сущности <typeparamref name="T"/> по запросу асинхронно
    /// </summary>
    /// <param name="predicate">Запрос для поиска</param>
    /// <returns>Множество сущностей, удовлетворяющих запросу</returns>
    public Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate); 

    /// <summary>
    /// Создает или обновляет сущность и сохраняет её в базе данных.
    /// </summary>
    /// <param name="entity">Сущность <typeparamref name="T"/> для сохранения или обновления в базе данных.</param>
    /// <returns>Задача, представляющая асинхронную операцию сохранения.</returns>
    public Task SaveAsync(T entity);

    /// <summary>
    /// Пакетно сохраняет список сущностей в базе данных.
    /// </summary>
    /// <param name="batch">Коллекция сущностей для пакетного сохранения.</param>
    /// <returns>Задача, представляющая асинхронную операцию пакетного сохранения.</returns>
    public Task SaveBatchAsync(IEnumerable<T> batch);

    /// <summary>
    /// Пакетно сохраняет список сущностей в базе данных.
    /// </summary>
    /// <param name="batch">Коллекция сущностей для пакетного сохранения.</param>
    /// <param name="batchSize">Объем сущностей, сохраняемых за одну транзакцию.</param>
    /// <returns>Задача, представляющая асинхронную операцию пакетного сохранения.</returns>
    public Task SaveBatchAsync(IEnumerable<T> batch, int batchSize);

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

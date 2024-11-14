using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

using API.Models.Base;

namespace API.Services.Base;

/// <summary>
/// Сервис для работы с сущностями типа <typeparamref name="T"/> в базе данных.
/// Предоставляет методы для добавления, удаления, обновления и получения сущностей.
/// </summary>
/// <typeparam name="T">Тип сущности, которая будет обрабатываться сервисом.</typeparam>
public class EntityService<T> : IEntityService<T> where T : class, IEntity {

    #region fields_ctor

    /// <summary>
    /// Контекст базы данных, используемый для взаимодействия с данными.
    /// </summary>
    protected readonly ApplicationDbContext _context;

    /// <summary>
    /// Логгер для записи событий и ошибок, связанных с операциями в сервисе.
    /// </summary>
    protected readonly ILogger<EntityService<T>> _logger;

    /// <summary>
    /// Фабрика для создания экземпляров сервисов работы с сущностями.
    /// </summary>
    protected readonly IEntityServiceFactory _serviceFactory;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="EntityService{T}"/> с указанными параметрами.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    /// <param name="logger">Логгер для записи событий и ошибок.</param>
    /// <param name="serviceFactory">Фабрика для создания экземпляров сервисов работы с сущностями.</param>
    public EntityService(ApplicationDbContext context, ILogger<EntityService<T>> logger, IEntityServiceFactory serviceFactory) { 
        _context = context; 
        _logger = logger; 
        _serviceFactory = serviceFactory;
    }

    /// <summary>
    /// Получает экземпляр <see cref="IEntityService{T1}"/> для указанного типа сущности с использованием фабрики сервисов.
    /// </summary>
    /// <typeparam name="T1">Тип сущности, для которого необходимо получить сервис. Должен реализовывать <see cref="IEntity"/>.</typeparam>
    /// <returns>Экземпляр <see cref="IEntityService{T1}"/> для работы с указанным типом сущности.</returns>
    public IEntityService<T1> GetEntityService<T1>() where T1 : class, IEntity {
        return _serviceFactory.Create<T1>();
    }

    #endregion

    /// <summary>
    /// Сохраняет сущность в базе данных асинхронно.
    /// Если сущность не отслеживается, она будет добавлена, иначе обновлена.
    /// </summary>
    /// <param name="entity">Сущность для сохранения.</param>
    public async Task SaveAsync(T entity) {
        EntityEntry<T> entry = _context.Entry(entity);
        if (entry.State == EntityState.Detached) 
            await _context.AddAsync(entity); // Добавление новой сущности
        else {
            entry.State = EntityState.Modified;
            entity.ChangeDate = DateTime.UtcNow; // Обновление сущности
        }
        await _context.SaveChangesAsync();
    }

    private const int defaultSavingBatchSize = 1000;

    /// <summary>
    /// Сохраняет сущности пакетно с использованием размера по умолчанию (1000).
    /// </summary>
    /// <param name="entities">Коллекция сущностей для сохранения.</param>
    public async Task SaveBatchAsync(IEnumerable<T> entities) => await SaveBatchAsync(entities, defaultSavingBatchSize);

    /// <summary>
    /// Сохраняет сущности пакетно с указанным размером пакета.
    /// Использует транзакцию для обеспечения целостности данных.
    /// </summary>
    /// <param name="entities">Коллекция сущностей для сохранения.</param>
    /// <param name="batchSize">Размер пакета для обработки.</param>
    public async Task SaveBatchAsync(IEnumerable<T> entities, int batchSize = defaultSavingBatchSize) {
        if (entities == null || !entities.Any()) return;

        using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync()) {
            List<T> package = entities.ToList();
            try {
                int currentIndex = 0;
                while (currentIndex < package.Count) {
                    List<T> batch = package.Skip(currentIndex).Take(batchSize).ToList(); 
                    await _context.AddRangeAsync(batch);  
                    await _context.SaveChangesAsync();  
                    _context.ChangeTracker.Clear();  //не положить ли это в finally? Как связан ChangeTracker и Rollback?

                    currentIndex += batch.Count; 
                }
                await transaction.CommitAsync(); // Завершаем транзакцию
            } catch {
                await transaction.RollbackAsync(); // Откат транзакции в случае ошибки
                throw;
            }
        }
    }

    /// <summary>
    /// Загружает сущность по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Найденная сущность или null, если не найдена.</returns>
    public async Task<T> LoadByIdAsync(long id) {
        return await _context.Set<T>().FindAsync(id);
    }

    /// <summary>
    /// Загружает сущность по идентификатору с включением связанных данных.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="includes">Свойства для включения в запрос (связанные сущности).</param>
    /// <returns>Найденная сущность с включенными данными или null.</returns>
    public async Task<T> LoadByIdWithRelatedDataAsync(long id, params Expression<Func<T, object>>[] includes) {
        IQueryable<T> query = _context.Set<T>().AsQueryable();
        foreach (var include in includes) 
            query = query.Include(include);  
        return await query.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    /// <summary>
    /// Возвращает все сущности данного типа.
    /// </summary>
    /// <returns>Список всех сущностей.</returns>
    public async Task<List<T>> GetAllAsync() {
        return await _context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Возвращает все сущности данного типа, отфильтрованные по выражению.
    /// </summary>
    /// <param name="predicate">Выражение для фильтрации сущностей.</param>
    /// <returns>Список отфильтрованных сущностей.</returns>
    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate) {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Возвращает все сущности с включением связанных данных.
    /// </summary>
    /// <param name="includes">Свойства для включения в запрос (связанные сущности).</param>
    /// <returns>Список всех сущностей с включенными данными.</returns>
    public async Task<List<T>> GetAllWithRelatedDataAsync(params Expression<Func<T, object>>[] includes) {
        IQueryable<T> query = _context.Set<T>().AsQueryable();
        foreach (var include in includes) 
            query = query.Include(include);  
        return await query.ToListAsync(); 
    }

    /// <summary>
    /// Удаляет коллекцию сущностей.
    /// </summary>
    /// <param name="entityList">Список сущностей для удаления.</param>
    public virtual async Task DeleteRangeAsync(IEnumerable<T> entityList) {
        if (entityList == null || !entityList.Any()) return;
        _context.Set<T>().RemoveRange(entityList);
        try { await _context.SaveChangesAsync(); } catch { throw; }
    }

    /// <summary>
    /// Удаляет сущность по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности для удаления.</param>
    public async Task DeleteAsync(long id) {
        T entity;
        if (!EntityExists(id, out entity))
            return;
        await DeleteAsync(entity);
    }

    /// <summary>
    /// Удаляет сущность.
    /// </summary>
    /// <param name="entity">Сущность для удаления.</param>
    public async Task DeleteAsync(T entity) {
        if (entity == null) return;
        _context.Set<T>().Remove(entity);
        try { await _context.SaveChangesAsync(); } catch { throw; }
    }

    /// <summary>
    /// Проверяет, существует ли сущность с указанным идентификатором в базе данных.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>True, если сущность существует; иначе false.</returns>
    public bool EntityExists(long id) {
        return _context.Set<T>().Any(x => x.Id == id);  
    }

    /// <summary>
    /// Проверяет существование сущности и возвращает результат с самой сущностью.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="entity">Найденная сущность.</param>
    /// <returns>True, если сущность существует; иначе false.</returns>
    public bool EntityExists(long id, out T entity) {
        entity = _context.Set<T>().Find(id);
        return entity != null;
    }
}

using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

using API.Models.Base;

namespace API.Services;

/// <summary>
/// Сервис для работы с сущностями
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public class EntityService<T> : IEntityService<T> where T : class, IEntity {

    protected readonly ApplicationDbContext _context;

    protected readonly ILogger<EntityService<T>> _logger;

    public EntityService(ApplicationDbContext context, ILogger<EntityService<T>> logger) { 
        _context = context; _logger = logger; 
    }

    /// <summary>
    /// Создать сущность (сохранить в БД)
    /// </summary>
    /// <returns></returns>
    public async Task SaveAsync(T entity) {
        // отслеживается ли объект контекстом (например, это обновление или новая запись)
        EntityEntry<T> entry = _context.Entry(entity);

        // Если объект не отслеживается, это новая сущность, добавляем в контекст
        if (entry.State == EntityState.Detached) 
            await _context.AddAsync(entity);
        // Иначе считаем, что это обновление
        else {
            entry.State = EntityState.Modified;
            entity.ChangeDate = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync();
    }

    private const int defaultSavingBatchSize = 1000;

    /// <summary>
    /// Пакетно созранить сущности
    /// </summary>
    /// <param name="batch"></param>
    /// <param name="bathcSize"></param>
    /// <returns></returns>
    public async Task SaveBatchAsync(IEnumerable<T> entities) => await SaveBatchAsync(entities, defaultSavingBatchSize);

    public async Task SaveBatchAsync(IEnumerable<T> entities, int batchSize = defaultSavingBatchSize) {
        if (entities == null || !entities.Any()) return;

        using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync()) {
            List<T> package = entities.ToList();
            try {
                int currentIndex = 0;
                while (currentIndex < package.Count) {
                    // Формируем пакет сущностей для добавления - Все оставшиеся элементы, если их меньше, чем batchSize
                    List<T> batch = package.Skip(currentIndex).Take(batchSize).ToList(); 
                    await _context.AddRangeAsync(batch);  
                    await _context.SaveChangesAsync();  
                    _context.ChangeTracker.Clear();  //Очищаем трекер изменений

                    currentIndex += batch.Count; 
                }
                await transaction.CommitAsync();
            } catch {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    /// <summary>
    /// Получить сущность по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<T> LoadByIdAsync(long id) {
        return await _context.Set<T>().FindAsync(id);
    }


    /// <summary>
    /// Загрузить по id
    /// С параметрами для "жадной" загрузки
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    public async Task<T> LoadByIdWithRelatedDataAsync(long id, params Expression<Func<T, object>>[] includes) {
        IQueryable<T> query = _context.Set<T>().AsQueryable();
        foreach (var include in includes) 
            query = query.Include(include);  
        return await query.FirstOrDefaultAsync(entity => entity.Id == id);
    }


    /// <summary>
    /// Получить все сущности
    /// </summary>
    /// <returns></returns>
    public async Task<List<T>> GetAllAsync() {
        return await _context.Set<T>().ToListAsync();
    }


    /// <summary>
    /// Получить все 
    /// С параметрами для "жадной" загрузки
    /// </summary>
    /// <param name="includes"></param>
    /// <returns></returns>
    public async Task<List<T>> GetAllWithRelatedDataAsync(params Expression<Func<T, object>>[] includes) {
        IQueryable<T> query = _context.Set<T>().AsQueryable();
        foreach (var include in includes) 
            query = query.Include(include);  // Применяем Include для каждого навигационного свойства
        return await query.ToListAsync(); 
    }
    

    /// <summary>
    /// Удалить список сущностей
    /// </summary>
    /// <param name="entityList"></param>
    /// <returns></returns>
    public virtual async Task DeleteRangeAsync(IEnumerable<T> entityList) {
        if (entityList == null || !entityList.Any()) return;
        _context.Set<T>().RemoveRange(entityList);
        try { await _context.SaveChangesAsync(); } catch { throw; }
    }


    /// <summary>
    /// Удалить асинхронно
    /// </summary>
    /// <param name="id">id сущности</param>
    /// <returns></returns>
    public async Task DeleteAsync(long id) {
        T @entity;
        if (!EntityExists(id, out @entity))
            return;
        await DeleteAsync(@entity);
    }


    /// <summary>
    /// Удалить сущеость асинхронно
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public  async Task DeleteAsync(T entity) {
        if (entity == null) return;
        _context.Set<T>().Remove(entity);
        try { await _context.SaveChangesAsync(); } catch { throw; }
    }


    /// <summary>
    /// Сущность есть в БД
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool EntityExists(long id) {
        return _context.Set<T>().Find(id) != null;
    }


    /// <summary>
    /// Сущность есть в БД, возвращает результат
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool EntityExists(long id, out T entity) {
        entity = _context.Set<T>().Find(id);
        return entity != null;
    }

}
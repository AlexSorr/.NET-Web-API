
using API.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

/// <summary>
/// Сервис для работы с сущностями
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public class EntityService<T> : IService<T> where T : class, IEntity {

    private readonly ApplicationDbContext _context;

    public EntityService(ApplicationDbContext context) { _context = context; }

    /// <summary>
    /// Получить сущность по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<T> GetEntityAsync(long id) {
        return await _context.Set<T>().FindAsync(id);
    }

    /// <summary>
    /// Получить все сущности
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<T>> GetAllAsync() {
        return await _context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Удаление сущности
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id) {
        T entity;
        if (!EntityExists(id, out entity))
            return;

        _context.Set<T>().Remove(entity);
        try {
            await _context.SaveChangesAsync();
        } catch {
            throw;
        }
    }

    /// <summary>
    /// Сущность существует
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool EntityExists(long id) {
        return _context.Set<T>().Find(id) != null;
    }

    /// <summary>
    /// Проверка на сущестование сущности 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool EntityExists(long id, out T entity) {
        entity = _context.Set<T>().Find(id);
        return entity != null;
    }

}

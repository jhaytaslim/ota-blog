using Infrastructure.Contracts;
using Infrastructure.Data.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Domain.Common;
using CommunityToolkit.Diagnostics;

namespace Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : AuditableEntity
{
    protected readonly ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }
    public virtual async Task AddAsync(TEntity entity)
    {
        Guard.IsNotNull(entity, nameof(entity));
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        await _context.Set<TEntity>().AddRangeAsync(entity);
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await QueryAll().CountAsync(predicate);
    }

    public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().Any(predicate);
    }
    public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return QueryAll().FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var query = this.QueryAll();
        return await query.AnyAsync(predicate);
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public virtual Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return QueryAll().FirstOrDefaultAsync(predicate);
    }

    public virtual Task<TEntity> FirstOrDefaultNoTracking(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public IQueryable<TEntity> FromSqlRaw(string query, object param = null)
    {
        return _context.Set<TEntity>().FromSqlRaw(query, param);
    }

    public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        query = query.Where(entity => !entity.Deleted);
        return query.Where(predicate);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity> GetByIdAsync(Guid id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);

        if (entity == null) return null;

        return entity;
    }

    public IQueryable<TEntity> QueryAll(Expression<Func<TEntity, bool>> predicate = null)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        query = query.Where(entity => !entity.Deleted);
        return predicate == null ? query : query.Where(predicate);
    }

    public virtual void Remove(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        _context.Set<TEntity>().Remove(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        Guard.IsNotNull(entity, nameof(entity));
        if (entity.Deleted) throw new InvalidOperationException("Item cannot be deleted");

        entity.Deleted = true;
    }

    public void RemoveRange(IEnumerable<TEntity> entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        _context.Set<TEntity>().RemoveRange(entity);
    }

    public virtual Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().SingleOrDefaultAsync(predicate);
    }

    public virtual Task<TEntity> SingleOrDefaultNoTracking(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(predicate);
    }

    public virtual void Update(TEntity entity)
    {

    }

    public virtual void UpdateRange(IEnumerable<TEntity> entity)
    {
        _context.Set<TEntity>().UpdateRange(entity);
    }
}


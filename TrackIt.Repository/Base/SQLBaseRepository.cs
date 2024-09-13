using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Domain.Contract;
using TrackIt.Persistence;

namespace TrackIt.Repository.Base;


public class SQLBaseRepository<T> : IBaseRepository<T> where T : class, IEntityBase
{
    private readonly AppDbContext context;

    public SQLBaseRepository(AppDbContext context)
    {
        this.context = context;
    }

    public virtual async Task<IEnumerable<T>?> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = context.Set<T>();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return await query.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = context.Set<T>();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return await query.AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await context.Set<T>().FirstOrDefaultAsync(n => n.Id == id);
        if (entity != null)
        {
            EntityEntry entityEntry = context.Entry<T>(entity);
            entityEntry.State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }
    }

    public virtual async Task UpdateAsync(T entity)
    {
        EntityEntry entityEntry = context.Entry<T>(entity);
        entityEntry.State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

}



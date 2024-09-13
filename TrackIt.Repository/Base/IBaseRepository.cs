using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Domain.Contract;

namespace TrackIt.Repository.Base;

    public interface IBaseRepository<T> where T : class, IEntityBase
    {
            //All
            Task<IEnumerable<T>?> GetAllAsync();

            //All with included properties
            Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);

            //Read
            Task<T?> GetByIdAsync(Guid id);

            Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties);
            //Create
            Task<T> AddAsync(T entity);

            //Update
            Task UpdateAsync(T entity);

            //Delete
            Task DeleteAsync(Guid id);
    }
    


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetValuesAsync();
        Task<T?> FindOneAsync(Expression<Func<T, bool>> filter);
        Task<T?> GetByIdAsync(object id);
        Task<int> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(object id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter);
        IQueryable<T> GetQueryable();
        Task<bool> UpdateRangeAsync(IEnumerable<T> entities);
    }
}

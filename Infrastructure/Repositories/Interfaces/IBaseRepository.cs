using ApplicationCore.Entities.Abstract;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> AnyAsync(int id);

        Task<T> GetByIdAsync(int id);

        Task<List<T>> GetAllAsync();

        Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> select,
                                                     Expression<Func<T, bool>> where = null,
                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
                                                     Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null);
    }
}

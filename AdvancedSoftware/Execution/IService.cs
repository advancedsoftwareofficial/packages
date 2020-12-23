using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdvancedSoftware.DataAccess.Execution
{
    public partial interface IService<T> where T : class
    {
        Task<T> AddAsync(T item);
        Task<List<T>> AddAsync(List<T> items);
        T Add(T item);
        List<T> Add(List<T> items);
        T Update(T item);
        List<T> Update(List<T> items);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> expression);
        Task<T> GetAsync(int id);
        T Get(int id);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        T Get(Expression<Func<T, bool>> expression);
        Task<bool> QueryAsync(Expression<Func<T, bool>> expression);
        bool Query(Expression<Func<T, bool>> expression);
        List<T> GetList(Expression<Func<T, bool>> expression);
        IQueryable<T> GetQueryable(Expression<Func<T, bool>> expression);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression);

        #region PaggedResults
        IQueryable<T> GetPagedQueryable(int page, int pageSize, Expression<Func<T, bool>> expression);
        #endregion
        int? Count(Expression<Func<T, bool>> expression);
        DbSet<T> AsDbSet();
        IQueryable<T> AsQueryable();
        IEnumerable<T> AsEnumerable();
        void Delete(int id);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}

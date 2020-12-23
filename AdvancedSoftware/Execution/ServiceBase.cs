using AdvancedSoftware.DataAccess.Database;
using AdvancedSoftware.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

namespace AdvancedSoftware.DataAccess.Execution
{
    public partial class ServiceBase<TEntity, TDbContext> : IService<TEntity> where TEntity : class, IEntityBase where TDbContext : DbContext
    {
        protected readonly AppDbContext<TDbContext> _dbContext;
        public ServiceBase(AppDbContext<TDbContext> dbContext)
        {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dbContext.ChangeTracker.LazyLoadingEnabled = false;
        }

        public virtual async Task<TEntity> AddAsync(TEntity item)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _dbContext.AddAsync(item);
                await _dbContext.SaveChangesAsync();
                scope.Complete();
            }
            return item;
        }
        public virtual TEntity Add(TEntity item)
        {
            try
            {
                using var scope = new TransactionScope();
                _dbContext.Add(item);
                _dbContext.SaveChanges();
                scope.Complete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return item;
        }
        public virtual async Task<List<TEntity>> AddAsync(List<TEntity> items)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _dbContext.AddRangeAsync(items);
                    await _dbContext.SaveChangesAsync();
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return items;
        }
        public virtual List<TEntity> Add(List<TEntity> items)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    _dbContext.AddRange(items);
                    _dbContext.SaveChanges();
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return items;
        }
        public virtual void Delete(int id)
        {
            using (var scope = new TransactionScope())
            {
                _dbContext.Remove(Get(id));
                _dbContext.SaveChanges();
                scope.Complete();
            }
        }
        public virtual void Delete(TEntity entity)
        {
            using (var scope = new TransactionScope())
            {
                if (entity != null)
                {
                    _dbContext.Update(entity);
                    _dbContext.SaveChanges();
                }
                scope.Complete();
            }
        }

        public void Delete(Expression<Func<TEntity, bool>> expression)
        {
            var entity = _dbContext.Set<TEntity>().FirstOrDefault(expression);
            if (entity != null)
            {
                _dbContext.Update(entity);
            }
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }
        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(expression);
        }
        public virtual TEntity Get(int id)
        {
            return _dbContext.Set<TEntity>().FirstOrDefault(x => x.Id == id);
        }
        public virtual TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            return _dbContext.Set<TEntity>().FirstOrDefault(expression);
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> expression)
        {
            return _dbContext.Set<TEntity>().Where(expression).ToList();
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbContext.Set<TEntity>().Where(expression).ToListAsync();
        }
        public IQueryable<TEntity> GetPagedQueryable(int page, int pageSize, Expression<Func<TEntity, bool>> expression)
        {
            return _dbContext.Set<TEntity>().Skip((page - 1) * pageSize).Take(pageSize)
                .Where(expression);
        }

        public virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> expression)
        {
            return _dbContext.Set<TEntity>().Where(expression);
        }
        public virtual async Task<bool> QueryAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbContext.Set<TEntity>().AnyAsync(expression);
        }
        public virtual bool Query(Expression<Func<TEntity, bool>> expression)
        {
            return _dbContext.Set<TEntity>().Any(expression);
        }

        public virtual TEntity Update(TEntity item)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _dbContext.Set<TEntity>().Update(item);
                _dbContext.SaveChanges();
                scope.Complete();
            }
            return item;
        }
        public virtual List<TEntity> Update(List<TEntity> items)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _dbContext.Set<TEntity>().UpdateRange(items);
                _dbContext.SaveChanges();
                scope.Complete();
            }
            return items;
        }

        public virtual int? Count(Expression<Func<TEntity, bool>> expression)
        {
            var count = _dbContext.Set<TEntity>().Count(expression);
            return count;
        }
        public virtual DbSet<TEntity> AsDbSet()
        {
            return _dbContext.Set<TEntity>();
        }
        public virtual IQueryable<TEntity> AsQueryable()
        {
            return _dbContext.Set<TEntity>().AsQueryable();
        }
        public virtual IEnumerable<TEntity> AsEnumerable()
        {
            return _dbContext.Set<TEntity>().AsEnumerable();
        }

        public virtual void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
        public virtual async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

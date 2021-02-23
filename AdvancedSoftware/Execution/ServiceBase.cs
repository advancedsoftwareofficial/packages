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
        public readonly AppDbContext<TDbContext> DbContext;
        public ServiceBase(AppDbContext<TDbContext> dbContext)
        {
            DbContext = dbContext;
            DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            DbContext.ChangeTracker.LazyLoadingEnabled = false;
        }

        public virtual async Task<TEntity> AddAsync(TEntity item)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await DbContext.AddAsync(item);
                await DbContext.SaveChangesAsync();
                scope.Complete();
            }
            return item;
        }
        public virtual TEntity Add(TEntity item)
        {
            try
            {
                using var scope = new TransactionScope();
                DbContext.Add(item);
                DbContext.SaveChanges();
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
                    await DbContext.AddRangeAsync(items);
                    await DbContext.SaveChangesAsync();
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
                    DbContext.AddRange(items);
                    DbContext.SaveChanges();
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
                DbContext.Remove(Get(id));
                DbContext.SaveChanges();
                scope.Complete();
            }
        }
        public virtual void Delete(TEntity entity)
        {
            using (var scope = new TransactionScope())
            {
                if (entity != null)
                {
                    DbContext.Update(entity);
                    DbContext.SaveChanges();
                }
                scope.Complete();
            }
        }

        public void Delete(Expression<Func<TEntity, bool>> expression)
        {
            var entity = DbContext.Set<TEntity>().FirstOrDefault(expression);
            if (entity != null)
            {
                DbContext.Update(entity);
            }
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            return await DbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }
        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbContext.Set<TEntity>().FirstOrDefaultAsync(expression);
        }
        public virtual TEntity Get(int id)
        {
            return DbContext.Set<TEntity>().FirstOrDefault(x => x.Id == id);
        }
        public virtual TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            return DbContext.Set<TEntity>().FirstOrDefault(expression);
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> expression)
        {
            return DbContext.Set<TEntity>().Where(expression).ToList();
        }
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbContext.Set<TEntity>().Where(expression).ToListAsync();
        }
        public IQueryable<TEntity> GetPagedQueryable(int page, int pageSize, Expression<Func<TEntity, bool>> expression)
        {
            return DbContext.Set<TEntity>().Skip((page - 1) * pageSize).Take(pageSize)
                .Where(expression);
        }

        public virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> expression)
        {
            return DbContext.Set<TEntity>().Where(expression);
        }
        public virtual async Task<bool> QueryAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbContext.Set<TEntity>().AnyAsync(expression);
        }
        public virtual bool Query(Expression<Func<TEntity, bool>> expression)
        {
            return DbContext.Set<TEntity>().Any(expression);
        }

        public virtual TEntity Update(TEntity item)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                DbContext.Set<TEntity>().Update(item);
                DbContext.SaveChanges();
                scope.Complete();
            }
            return item;
        }
        public virtual List<TEntity> Update(List<TEntity> items)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                DbContext.Set<TEntity>().UpdateRange(items);
                DbContext.SaveChanges();
                scope.Complete();
            }
            return items;
        }

        public virtual int? Count(Expression<Func<TEntity, bool>> expression)
        {
            var count = DbContext.Set<TEntity>().Count(expression);
            return count;
        }
        public virtual DbSet<TEntity> AsDbSet()
        {
            return DbContext.Set<TEntity>();
        }
        public virtual IQueryable<TEntity> AsQueryable()
        {
            return DbContext.Set<TEntity>().AsQueryable();
        }
        public virtual IEnumerable<TEntity> AsEnumerable()
        {
            return DbContext.Set<TEntity>().AsEnumerable();
        }

        public virtual void SaveChanges()
        {
            DbContext.SaveChanges();
        }
        public virtual async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}

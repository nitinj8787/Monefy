using Monefy.Data.Access.Interfaces;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Monefy.Data.Access.DAL
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext;
        public EFUnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add<T>(T obj) where T : class
        {
            var set = _dbContext.Set<T>();
            set.Add(obj);
        }

        public void Attach<T>(T obj) where T : class
        {
            var set = _dbContext.Set<T>();
            set.Attach(obj);
        }

        public DbContext Context => _dbContext;

        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Snapshot)
        {
            return  new DbTransaction(_dbContext.Database.BeginTransaction(IsolationLevel.Snapshot));
            
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Remove<T>(T obj) where T : class
        {
            var set = _dbContext.Set<T>();
            set.Remove(obj);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        public void Update<T>(T obj) where T : class
        {
            var set = _dbContext.Set<T>();
            set.Update(obj);
        }
    }
}

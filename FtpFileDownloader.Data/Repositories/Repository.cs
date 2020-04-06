using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FtpFileDownloaderData.Context;
using Microsoft.EntityFrameworkCore;

namespace FtpFileDownloaderData.Repositories {
    public class Repository<TEntity>: IRepository<TEntity> where TEntity : class {
        private readonly FilesAppContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(FilesAppContext context) {
            _context = context;
            _dbSet = context.GetDbSet<TEntity>();
        }
        
        public async Task AddAsync(TEntity item, bool saveImmediately = true) {
            _dbSet.Add(item);
            if(saveImmediately) await SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id) {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, 
                                                           IEnumerable<string> includedProps = null) {
            var result = this._dbSet.AsQueryable();

            if(includedProps != null) {
                foreach(var prop in includedProps) {
                    result = result.Include(prop);
                }
            }

            return
                predicate == null
                    ? await Task.Run(() => result)
                    : await Task.Run(() => result.Where(predicate));
        }

        public async Task<bool> UpdateAsync(TEntity item) {
            await _dbSet.AddAsync(item);
            return await SaveChangesAsync();
        }

        public async Task<bool> RemoveAsync(int id) {
            var entity = await GetByIdAsync(id);
            _dbSet.Remove(entity);
            return await SaveChangesAsync();
        }

        public virtual async Task<bool> SaveChangesAsync() {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}

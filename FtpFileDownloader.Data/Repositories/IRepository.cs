using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FtpFileDownloaderData.Repositories {
    public interface IRepository<TEntity> {
        Task AddAsync(TEntity item, bool saveImmediately = true);
        Task<TEntity> GetByIdAsync(int id);

        Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, 
                                              IEnumerable<string> includedProps = null);
        Task<bool> UpdateAsync(TEntity item);
        Task<bool> RemoveAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}

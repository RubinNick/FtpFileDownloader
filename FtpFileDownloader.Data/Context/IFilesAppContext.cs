using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FtpFileDownloaderData.Context {
    public interface IFilesAppContext {
        DbSet<T> GetDbSet<T>() where T : class;
        
        void SetModified(object entity);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

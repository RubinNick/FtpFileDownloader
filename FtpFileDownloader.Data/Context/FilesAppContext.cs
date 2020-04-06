using FtpFileDownloaderCommon.Models;
using Microsoft.EntityFrameworkCore;

namespace FtpFileDownloaderData.Context {
    public class FilesAppContext : DbContext, IFilesAppContext {
        public FilesAppContext(DbContextOptions<FilesAppContext> options) : base(options) {}
        
        public DbSet<FileModel> Files { get; set; }

        public virtual DbSet<T> GetDbSet<T>() where T : class {
            return Set<T>();
        }

        public void SetModified(object entity) {
            Entry(entity).State = EntityState.Modified;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<FileModel>()
                        .Property(p => p.Id).ValueGeneratedOnAdd();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}

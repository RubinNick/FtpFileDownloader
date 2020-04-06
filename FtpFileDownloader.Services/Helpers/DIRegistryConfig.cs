using FtpFileDownloaderInterfaces.Helpers;
using FtpFileDownloaderInterfaces.Services;
using FtpFileDownloaderServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FtpFileDownloaderServices.Helpers
{
    public class DIRegistryConfig
    {
        public static void RegisterBusinessDependencies(IServiceCollection services)
        {
            services.AddScoped<ICryptoHelper, CryptoHelper>();
            services.AddScoped<IFileUploaderService, FileUploaderService>();
        }
    }
}
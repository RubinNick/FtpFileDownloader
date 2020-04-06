using System.Threading;
using System.Threading.Tasks;
using FluentFTP;
using FtpFileDownloaderUtils;

namespace FtpFileDownloader.Console
{
    class Program
    {
        private static Mutex consoleMutex = new Mutex();
        private static SemaphoreSlim clientSemaphore = new SemaphoreSlim(0, 5);
        
        static async Task Main(string[] args)
        {
            System.Console.WriteLine("Hello! Press '1' to select our ftp server!");
            var ftpHost = "ftp://f19-preview.royalwebhosting.net/";

            System.Console.WriteLine("Bye-bye, man!");
            
            System.Console.WriteLine("Enter username (1 = default user: 3378099_test)");
            var consoleUsername = System.Console.ReadLine();
            var ftpUsername = string.Equals(consoleUsername, "1") ? "3378099_test" : consoleUsername;
            
            System.Console.WriteLine("Enter password (1 = default password: test123HELLO)");
            var consolePassword = System.Console.ReadLine();
            var ftpPassword = Equals(consolePassword, "1") ? "test123HELLO" : consolePassword;

            var client = new FtpClientUtil(ftpHost, 21, ftpUsername, ftpPassword);
            var currentConnection = await client.ConnectAsync();
            var ftpList = await client.GetFilesDirectoriesListAsync(currentConnection);

            foreach (var item in ftpList)
            {
                if (item.Type != FtpFileSystemObjectType.File) continue;
                System.Console.WriteLine($"Type: {item.Type}, Path: {item.FullName}, Size: {item.Size}");
            }

            System.Console.WriteLine($"Starting downloads...");
            System.Console.CursorVisible = false;
            
            //utilize generic client, before download starts
            currentConnection.Dispose();
            
            Parallel.ForEach(ftpList, async (item) =>
            {
                if (item.Type != FtpFileSystemObjectType.File && !item.Name.Contains("important")) return;

                await clientSemaphore.WaitAsync();
                using var uClient = new FtpClient(ftpHost, ftpUsername, ftpPassword);
                await client.ConnectAsync();
                
                var ftpPath = item.FullName;
                var threadId = Thread.CurrentThread.ManagedThreadId;
                System.Console.WriteLine($"Downloading {item.Name} in thread {threadId}: 0 bytes of {item.Size}");
                var stringPosition = System.Console.CursorTop - 1;
                await client.DownloadFileAsync(uClient, ftpPath, item, stringPosition, consoleMutex, true);
                clientSemaphore.Release();
            });

            //if (downloadResult.IsCompleted)
            //{
            //    System.Console.SetCursorPosition(0, System.Console.CursorTop);
            //    System.Console.CursorVisible = true;
            //}
            
            System.Console.ReadKey();
        }
    }
}
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentFTP;

namespace FtpFileDownloaderUtils
{
    public class FtpClientUtil
    {
        private readonly string _remoteHost;
        private readonly int _port;
        private readonly string _remoteUser;
        private readonly string _remotePassword;

        public FtpClientUtil(string remoteHost, int port, string remoteUser, string remotePassword)
        {
            _remoteHost = remoteHost;
            _port = port;
            _remoteUser = remoteUser;
            _remotePassword = remotePassword;
        }

        public async Task<FtpClient> ConnectAsync()
        {
            var client = new FtpClient(_remoteHost, _port, _remoteUser, _remotePassword );
            
            try
            {
                await client.ConnectAsync();
            }
            catch (FtpException ex)
            {
                throw new Exception(ex.Message);
            }
            
            return client;
        }

        public async Task DisconnectAsync(FtpClient client)
        {
            await client.DisconnectAsync();
        }

        public async Task<FtpListItem[]> GetFilesDirectoriesListAsync(FtpClient currentConnection, string path = "")
        {
            var items = await currentConnection.GetListingAsync(path);
            //typezied! get modifiedTime. size, hash, type and etc foreach
            return items;
        }

        public async Task DownloadFilesAsync(FtpClient currentConnection, string localPath, 
            string[] paths, CancellationToken token = default)
        {
            IProgress<FtpProgress> progress = new Progress<FtpProgress>();
            var downloadFilesCount = await currentConnection.DownloadFilesAsync(localPath, paths, FtpLocalExists.Overwrite,
                FtpVerify.OnlyChecksum, FtpError.Throw, token, progress);
        }
        
        public async Task DownloadFileAsync(FtpClient currentConnection, string ftpPath, FtpListItem file,
            int stringPosition, Mutex consoleMutex, bool dispose, CancellationToken token = default)
        {
            var localPath = $@"D:\VSProjects\FtpFileDownloader\DownloadFolder\{file.FullName.Substring(1)}";
            IProgress<FtpProgress> progress = new Progress<FtpProgress>(p =>
            {
                consoleMutex.WaitOne();
                
                    Console.SetCursorPosition(0, stringPosition);
                    
                    var line = $"Downloading {file.Name} in thread {Thread.CurrentThread.ManagedThreadId}: 0 bytes of {file.Size}";
                    var newText = String.Empty;
                    var startIndex = 0;
                    var endIndex = 0;
                    
                    if (p.Progress == 1)
                    {
                        startIndex = line.IndexOf(": ");
                        endIndex = line.IndexOf(" bytes");
                        newText = "Successful completed!";
                    }
                    else
                    {
                        startIndex = line.IndexOf(": ")+2;
                        endIndex = line.IndexOf(" bytes");
                        newText = p.TransferredBytes.ToString();
                    }
                    
                    var newLine = line.Substring(0, startIndex) + newText + line.Substring(endIndex, line.Length-endIndex);
                    Console.WriteLine(newLine);
                
                consoleMutex.ReleaseMutex();
            });

            try
            {
                await currentConnection.DownloadFileAsync(localPath, ftpPath, FtpLocalExists.Overwrite,
                    FtpVerify.None, progress, token);

            }
            catch (FtpException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dispose)
                {
                    currentConnection.Dispose();
                }
            }
        }


        public async Task<Stream> GetFileStream(FtpClient currentConnection, string remotePath)
        {
            var stream = await currentConnection.OpenReadAsync(remotePath);
            return stream;
        }
    }
}
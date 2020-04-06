using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FtpFileDownloaderCommon.Models;
using FtpFileDownloaderData.Repositories;
using FtpFileDownloaderInterfaces.Services;

namespace FtpFileDownloaderServices.Services
{
    public class FileUploaderService : IFileUploaderService
    {
        private readonly IRepository<FileModel> _filesRepository;

        public FileUploaderService(IRepository<FileModel> filesRepository)
        {
            _filesRepository = filesRepository;
        }

        public async Task<ParallelLoopResult> UploadFilesAsync(string[] filesPaths)
        {
            var filesList = new List<FileModel>();
            var parallelLoopResult = Parallel.ForEach(filesPaths,
                async fileName =>
                {
                    var filePath = fileName;
                    var fileBody = await File.ReadAllTextAsync(filePath);
                    var fileInfo = new FileInfo(filePath);

                    var bodyArrLength = fileBody.Length / 44 + (fileBody.Length % 44 == 0 ? 0 : 1);
                    string[] bodyArr = new string[bodyArrLength];

                    for (int i = 0; i < bodyArrLength; i++)
                    {
                        bodyArr[i] = i == bodyArrLength - 1 ? fileBody : fileBody.Substring(i * 44, 44);
                    }

                    var encodedBody = string.Empty;
                    var encodeLoopResult = Parallel.ForEach(bodyArr,
                        arrElem =>
                        {
                            //var encoded = await _cryptoHelper.EncodeText(arrElem);
                            var encoded = arrElem;
                            encodedBody += encoded;
                        });
                    
                    if(encodeLoopResult.IsCompleted == false) return;
                    
                    var file = new FileModel
                    {
                        FileName = fileInfo.Name,
                        FileBody = Encoding.Unicode.GetBytes(encodedBody)
                    };
                    
                    filesList.Add(file);
                });

            if (parallelLoopResult.IsCompleted)
            {
                foreach (var file in filesList)
                {
                    await _filesRepository.AddAsync(file);
                }
            }

            return parallelLoopResult;
        }
    }
}
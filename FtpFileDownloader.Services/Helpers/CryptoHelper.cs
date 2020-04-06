using Chilkat;
using FtpFileDownloaderCommon.Models;
using FtpFileDownloaderInterfaces.Helpers;
using Microsoft.Extensions.Options;

namespace FtpFileDownloaderServices.Helpers
{
    public class CryptoHelper : ICryptoHelper
    {
        private readonly TwofishConfigModel _configModel;
        private Crypt2 _crypt2 { get; }

        public CryptoHelper(IOptions<TwofishConfigModel> configModel)
        {
            _configModel = configModel.Value;
            _crypt2 = ConfigureCrypt(_configModel);
        }

        private Crypt2 ConfigureCrypt(TwofishConfigModel config)
        {
            var crypt = new Crypt2
            {
                CryptAlgorithm = config.CryptAlgorithm,
                CipherMode = config.CipherMode,
                KeyLength = config.KeyLength,
                PaddingScheme = config.PaddingScheme,
                EncodingMode = config.EncodingMode
            };
            
            crypt.SetEncodedIV(config.IvHex,config.EncodingMode);
            crypt.SetEncodedKey(config.KeyHex,config.EncodingMode);

            return crypt;
        }
        
        public string EncodeText(string text) {
            var status = new Chilkat.Global().UnlockStatus;
            return _crypt2.EncryptStringENC(text);
        }

        public string DecodeText(string encryptedText) {
            return _crypt2.DecryptStringENC(encryptedText);
        }
    }
}
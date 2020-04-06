namespace FtpFileDownloaderInterfaces.Helpers
{
    public interface ICryptoHelper
    {
        string EncodeText(string text);
        string DecodeText(string encryptedText);
    }
}
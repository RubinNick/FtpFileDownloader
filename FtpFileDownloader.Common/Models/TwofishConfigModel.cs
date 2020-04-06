namespace FtpFileDownloaderCommon.Models {
    public class TwofishConfigModel {
        public string CryptAlgorithm { get; set; }
        public string CipherMode { get; set; }
        public int KeyLength { get; set; }
        public int PaddingScheme { get; set; }
        public string EncodingMode { get; set; }
        public string IvHex { get; set; }
        public string KeyHex { get; set; }
    }
}

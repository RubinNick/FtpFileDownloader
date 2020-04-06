namespace FtpFileDownloaderCommon.Models {
    public class FileModel : BaseModel {
        private string _fileName;
        private string _extension;
        private int _size;
        private byte[] _fileBody;
        private bool _isCrypted;
        
        public string FileName {
            get => _fileName;
            set {
                _fileName = value;
            }
        }

        public string Extension {
            get => _extension;
            set {
                _extension = value;
            }
        }

        public int Size {
            get => _size;
            set {
                _size = value;
            }
        }

        public byte[] FileBody {
            get => _fileBody;
            set {
                _fileBody = value;
            }
        }

        public bool IsCrypted {
            get => _isCrypted;
            set {
                _isCrypted = value;
            }
        }
    }
}

namespace FtpFileDownloaderCommon.Models {
    public class BaseModel {
        private int _id;
        private string _uploadedDate;
        
        public int Id { get => _id; set => _id = value; }

        public string UploadedDate {
            get => _uploadedDate;
            set {
                _uploadedDate = value;
            }
        }
    }
}

namespace Transportation.Buisness._0.Common.FileManager
{
    public class UploadFileResult
    {
        public bool IsSucceeded { get; private set; }
        public string Message { get; private set; }
        public string Path { get; private set; }
        public string FileName { get; private set; }
        public string OriginalFileName { get; private set; }
        public long FileSize { get; private set; }

        private UploadFileResult() { }

        public static UploadFileResult Success(string path, string fileName, string originalFileName, long fileSize)
            => new() { IsSucceeded = true, Path = path, FileName = fileName, OriginalFileName = originalFileName, FileSize = fileSize };

        public static UploadFileResult Failed(string message)
            => new() { IsSucceeded = false, Message = message };
    }

    public class DeleteFileResult
    {
        public bool IsSucceeded { get; private set; }
        public string Message { get; private set; }

        private DeleteFileResult() { }

        public static DeleteFileResult Success()
            => new() { IsSucceeded = true };

        public static DeleteFileResult Failed(string message)
            => new() { IsSucceeded = false, Message = message };
    }

    public class DownloadFileResult
    {
        public bool IsSucceeded { get; private set; }
        public string Message { get; private set; }
        public byte[] Bytes { get; private set; }
        public string ContentType { get; private set; }
        public string FileName { get; private set; }

        private DownloadFileResult() { }

        public static DownloadFileResult Success(byte[] bytes, string contentType, string fileName)
            => new() { IsSucceeded = true, Bytes = bytes, ContentType = contentType, FileName = fileName };

        public static DownloadFileResult Failed(string message)
            => new() { IsSucceeded = false, Message = message };
    }
}

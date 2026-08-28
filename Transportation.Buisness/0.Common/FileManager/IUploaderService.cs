using Microsoft.AspNetCore.Http;

namespace Transportation.Buisness._0.Common.FileManager
{
    public interface IFileService
    {
        Task<UploadFileResult> UploadAsync(IFormFile file, string folder);
        Task<List<UploadFileResult>> UploadManyAsync(List<IFormFile> files, string folder);
        DeleteFileResult Delete(string relativePath);
        void DeleteMany(List<string> relativePaths);
        DownloadFileResult Download(string relativePath);
        bool Exists(string relativePath);
    }
}

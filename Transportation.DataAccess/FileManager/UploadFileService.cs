using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Transportation.Buisness._0.Common.Constants;
using Transportation.Buisness._0.Common.FileManager;

namespace Transportation.DataAccess.FileManager
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IWebHostEnvironment _env;

        public UploadFileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        // آپلود یک فایل
        public async Task<UploadFileResult> UploadAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return UploadFileResult.Failed("فایلی انتخاب نشده است");

            var allowedExtensions = GetAllowedExtensions(folder);
            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(ext))
                return UploadFileResult.Failed($"فرمت فایل مجاز نیست. فرمت‌های مجاز: {string.Join(", ", allowedExtensions)}");

            if (!IsFileSizeValid(file, folder))
                return UploadFileResult.Failed($"حجم فایل بیش از حد مجاز است");

            var folderPath = Path.Combine(
                _env.WebRootPath,
                UploadFilesPath.Uploads.TrimStart('/'),
                folder);

            Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var relativePath = $"{UploadFilesPath.Uploads}/{folder}/{fileName}";

            return UploadFileResult.Success(relativePath, fileName, file.FileName, file.Length);
        }

        // آپلود چند فایل
        public async Task<List<UploadFileResult>> UploadManyAsync(List<IFormFile> files, string folder)
        {
            var results = new List<UploadFileResult>();

            if (files == null || !files.Any())
                return results;

            foreach (var file in files)
                results.Add(await UploadAsync(file, folder));

            return results;
        }

        // حذف فایل
        public DeleteFileResult Delete(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return DeleteFileResult.Failed("مسیر فایل نامعتبر است");

            var fullPath = Path.Combine(
                _env.WebRootPath,
                relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(fullPath))
                return DeleteFileResult.Failed("فایل یافت نشد");

            File.Delete(fullPath);

            return DeleteFileResult.Success();
        }

        // حذف چند فایل
        public void DeleteMany(List<string> relativePaths)
        {
            if (relativePaths == null || !relativePaths.Any()) return;
            foreach (var path in relativePaths)
                Delete(path);
        }

        // دانلود فایل
        public DownloadFileResult Download(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return DownloadFileResult.Failed("مسیر فایل نامعتبر است");

            var fullPath = Path.Combine(
                _env.WebRootPath,
                relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(fullPath))
                return DownloadFileResult.Failed("فایل یافت نشد");

            var fileName = Path.GetFileName(fullPath);
            var contentType = GetContentType(fileName);
            var bytes = File.ReadAllBytes(fullPath);

            return DownloadFileResult.Success(bytes, contentType, fileName);
        }

        // بررسی وجود فایل
        public bool Exists(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return false;

            var fullPath = Path.Combine(
                _env.WebRootPath,
                relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            return File.Exists(fullPath);
        }

        // ==================================================
        // Private helpers
        // ==================================================

        private static string[] GetAllowedExtensions(string folder) => folder switch
        {
            UploadFilesPath.Products => new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" },
            _ => new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".pdf", ".zip" }
        };

        private static long GetMaxFileSize(string folder) => folder switch
        {
            UploadFilesPath.Products => 5 * 1024 * 1024,  // 5MB
            _ => 10 * 1024 * 1024  // 10MB
        };

        private static bool IsFileSizeValid(IFormFile file, string folder)
            => file.Length <= GetMaxFileSize(folder);

        private static string GetContentType(string fileName)
        {
            return Path.GetExtension(fileName).ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".pdf" => "application/pdf",
                ".zip" => "application/zip",
                _ => "application/octet-stream"
            };
        }
    }
}

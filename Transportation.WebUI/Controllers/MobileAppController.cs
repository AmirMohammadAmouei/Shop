using Microsoft.AspNetCore.Mvc;
using Transportation.Buisness._0.Common.FileManager;
using Transportation.Buisness.Services.MobileApps;
using Transportation.Buisness.Services.MobileApps.Dtos;

namespace Transportation.WebUI.Controllers
{
    public class MobileAppController : Controller
    {
        private readonly MobileAppService _mobileAppService;
        private readonly IFileService _fileService;

        public MobileAppController(MobileAppService mobileAppService, IFileService fileService)
        {
            _mobileAppService = mobileAppService;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(MobileAppListRequestDto request)
        {
            var mobileApps = await _mobileAppService.List(request);

            if (mobileApps.IsSucceeded)
                return View(mobileApps.Data);

            return View(new MobileAppListRequestDto());
        }

        [HttpGet]
        public async Task<IActionResult> Download(long id)
        {
            var pathResult = await _mobileAppService.GetFilePath(id);
            if (!pathResult.IsSucceeded) return NotFound();

            var download = _fileService.Download(pathResult.Data);
            if (!download.IsSucceeded) return NotFound();

            return File(download.Bytes, download.ContentType, download.FileName);
        }

        [HttpGet]
        public async Task<IActionResult> GetIcon(long id)
        {
            var pathResult = await _mobileAppService.GetIconPath(id);
            if (!pathResult.IsSucceeded) return NotFound();

            var download = _fileService.Download(pathResult.Data);
            if (!download.IsSucceeded) return NotFound();

            return File(download.Bytes, download.ContentType, download.FileName);
        }
    }
}

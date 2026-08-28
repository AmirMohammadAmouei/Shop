using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Transportation.Buisness._0.Common;
using Transportation.Buisness._0.Common.FileManager;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.MobileApps;
using Transportation.Buisness.Services.MobileApps.Dtos;

namespace Transportation.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            var response = await _mobileAppService.List(request);
            
            var apps = response.IsSucceeded ? response.Data : new SPFOutPutDto<MobileAppListResponseDto>();

            return View(apps);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails(long id)
        {
            var response = await _mobileAppService.GetDetails(id);

            if (!response.IsSucceeded)
                return BadRequest(response.Message);

            return Ok(response.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateMobileAppDto request, IFormFile File, IFormFile Icon)
        {

            if (File == null || File?.Length == 0)
                return BadRequest("فایل ارسالی نامعتبر است");

            if (Icon == null || Icon?.Length == 0)
                return BadRequest("آیکون بارگزاری شده نامعتبر است");

            request.File = File;
            request.Icon = Icon;

            var response = await _mobileAppService.Create(request);

            if (!response.IsSucceeded)
                return BadRequest(response.Message);

            return Ok(response.Data);
        }


        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateMobileAppDto request, IFormFile file, IFormFile icon)
        {
            request.File = file;
            request.Icon = icon;
            var response = await _mobileAppService.UpdateMobileApp(request);

            if (!response.IsSucceeded)
                return BadRequest(response.Message);

            return Ok();
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

        public async Task<IActionResult> Delete(long id)
        {
            var response = await _mobileAppService.Delete(id);

            if (response.IsSucceeded)
                return Ok();

            return BadRequest(response.Message);
        }

    }
}

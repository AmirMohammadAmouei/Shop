using Microsoft.AspNetCore.Mvc;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.MobileApps;
using Transportation.Buisness.Services.MobileApps.Dtos;

namespace Transportation.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MobileAppController : Controller
    {
        private readonly MobileAppService _mobileAppService;

        public MobileAppController(MobileAppService mobileAppService)
        {
            _mobileAppService = mobileAppService;
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
        public async Task<IActionResult> Create([FromForm] CreateMobileAppDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage)
                  .FirstOrDefault());

            var response = await _mobileAppService.Create(request);

            if (!response.IsSucceeded)
                return BadRequest(response.Message);

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateMobileAppDto request)
        {
            var response = await _mobileAppService.UpdateMobileApp(request);

            if (!response.IsSucceeded)
                return BadRequest(response.Message);

            return Ok();
        }
    }
}

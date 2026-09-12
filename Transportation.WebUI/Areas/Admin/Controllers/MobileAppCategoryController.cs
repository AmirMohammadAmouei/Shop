using Microsoft.AspNetCore.Mvc;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.MobileAppCategories;
using Transportation.Buisness.Services.MobileAppCategories.Dtos;

namespace Transportation.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MobileAppCategoryController : Controller
    {
        private readonly MobileAppCategoryService _mobileAppCategoryService;

        public MobileAppCategoryController(MobileAppCategoryService mobileAppCategoryService)
        {
            _mobileAppCategoryService = mobileAppCategoryService;
        }

        public async Task<IActionResult> Index(MobileAppCategoryListRequestDto request)
        {
            var response = await _mobileAppCategoryService.List(request);

            if (!response.IsSucceeded)
            {
                TempData["ErrorMessage"] = response.Message ?? "خطا در دریافت اطلاعات";
                return View(new SPFOutPutDto<MobileAppCategoryListResponseDto>());
            }

            return View(response.Data);
        }


        [HttpGet]
        public async Task<IActionResult> GetDetails(long id)
        {
            var response = await _mobileAppCategoryService.GetDetails(id);

            if (response.IsSucceeded)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateMobileAppCategoryDto request)
        {
            var response = await _mobileAppCategoryService.Create(request);


            if (response.IsSucceeded)
                return Ok();

            return BadRequest(response.Message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromBody] UpdateMobileAppCategoryDto request)
        {
            var response = await _mobileAppCategoryService.Update(request);

            if (response.IsSucceeded)
                return Ok();

            return BadRequest(response.Message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var response = await _mobileAppCategoryService.Delete(id);

            if (response.IsSucceeded)
                return Ok();

            return BadRequest(response.Message);
        }
    }
}

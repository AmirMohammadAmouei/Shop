using Microsoft.AspNetCore.Mvc;
using Transportation.Buisness.Services.AboutUs;
using Transportation.Buisness.Services.AboutUs.Dtos;

namespace Transportation.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutUsController : Controller
    {
        private readonly AboutUsService _aboutUsService;

        public AboutUsController(AboutUsService aboutUsService)
        {
            _aboutUsService = aboutUsService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _aboutUsService.GetDetails();
            var aboutUs = result.IsSucceeded
                ? result.Data
                : new AboutUsResponseDto();

            return View(aboutUs);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateAboutUsDto request)
        {
            var result = await _aboutUsService.Create(request);

            if (!result.IsSucceeded)
                return BadRequest(result.Message);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateAboutUsDto request)
        {
            var result = await _aboutUsService.Update(request);

            if (!result.IsSucceeded)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.CompanyFeatures;
using Transportation.Buisness.Services.CompanyFeatures.Dtos;

namespace Transportation.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyFeatureController : Controller
    {
        private readonly CompanyFeatureService _companyFeatureService;

        public CompanyFeatureController(CompanyFeatureService companyFeatureService)
        {
            _companyFeatureService = companyFeatureService;
        }

        public async Task<IActionResult> Index(CompanyFeatureListRequestDto request)
        {
            var response = await _companyFeatureService.List(request);

            var feature = response.IsSucceeded ? response.Data : new SPFOutPutDto<CompanyFeatureListResponseDto>();

            return View(feature);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails(long id)
        {
            var response = await _companyFeatureService.GetDetails(id);

            if (response.IsSucceeded)
                return Ok(response.Data);

            return BadRequest(response.Message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateCompanyFeatureDto request)
        {
            var response = await _companyFeatureService.Create(request);

            if (response.IsSucceeded)
                return Ok();

            return BadRequest(response.Message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyFeatureDto request)
        {
            var response = await _companyFeatureService.Update(request);

            if (response.IsSucceeded)
                return Ok();

            return BadRequest(response.Message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var response = await _companyFeatureService.Delete(id);

            if (response.IsSucceeded)
                return Ok();

            return BadRequest(response.Message);
        }
    }
}

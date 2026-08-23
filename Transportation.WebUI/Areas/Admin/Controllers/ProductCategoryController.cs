using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.ProductCategories;
using Transportation.Buisness.Services.ProductCategories.Dto;

namespace Transportation.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly ProductCategoryService _productCategoryService;

        public ProductCategoryController(ProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public async Task<IActionResult> Index(ProductCategoryListRequestDto request)
        {
            var response = await _productCategoryService.List(request);

            if (!response.IsSucceeded)
            {
                TempData["ErrorMessage"] = response.Message ?? "خطا در دریافت اطلاعات";
                return View(new SPFOutPutDto<ProductCategoryListResponseDto>());
            }

            ViewBag.SearchTerm = request.SearchTerm;

            return View(response.Data);
        }


        [HttpGet]
        public async Task<IActionResult> ProductCategoryDetails(long id)
        {
            var response = await _productCategoryService.GetDetails(id);

            if (!response.IsSucceeded)
            {
                TempData["ErrorMessage"] = response.Message ?? "خطا در دریافت اطلاعات";
                return View(new ProductCategoryDetailsDto());
            }

            return View(response.Data);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateProductCategoryDto request)
        {

            if (!ModelState.IsValid)
                return View(request);

            var response = await _productCategoryService.Create(request);

            if (!response.IsSucceeded)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromBody] UpdateProductCategoryDto request)
        {

            if (!ModelState.IsValid)
                return View(request);

            var response = await _productCategoryService.Update(request);

            if (!response.IsSucceeded)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                return View(request);
            }

            return Ok();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var response = await _productCategoryService.DeleteProductCategory(id);

            if (!response.IsSucceeded)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                return BadRequest(response.Message);
            }

            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.AboutUs;
using Transportation.Buisness.Services.AboutUs.Dtos;
using Transportation.Buisness.Services.ProductCategories;
using Transportation.Buisness.Services.ProductCategories.Dto;
using Transportation.Buisness.Services.Products;
using Transportation.Buisness.Services.Products.Dto;
using Transportation.WebUI.Models;

namespace Transportation.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly AboutUsService _aboutUsService;
        private readonly ProductCategoryService _productCategoryService;
        private readonly ProductService _productService;


        public HomeController(AboutUsService aboutUsService, ProductCategoryService productCategoryService, ProductService productService)
        {
            _aboutUsService = aboutUsService;
            _productCategoryService = productCategoryService;
            _productService = productService;
        }

        public async Task<IActionResult> Index(ProductCategoryListRequestDto request)
        {
            var aboutUsResult = await _aboutUsService.GetDetails();
            var categoriesResult = await _productCategoryService.List(request);

            var model = new HomeViewModel
            {
                AboutUs = aboutUsResult.IsSucceeded
                    ? aboutUsResult.Data
                    : new AboutUsResponseDto(),
                Categories = categoriesResult.IsSucceeded
                    ? categoriesResult.Data.Items
                    : new List<ProductCategoryListResponseDto>()
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> GetProductsByCategory(long categoryId)
        {
            List<ProductListResponseDto> products;

            if (categoryId == 0)
            {
                var result = await _productService.List(new ProductListRequestDto());
                products = result.IsSucceeded ? result.Data.Items : new List<ProductListResponseDto>();
            }
            else
            {
                var result = await _productService.GetProductsByCategoryId(categoryId,
                    new ProductListRequestDto { PageSize = 4 });
                products = result.IsSucceeded ? result.Data.Items : new List<ProductListResponseDto>();
            }

            return Json(new { isSucceded = true, items = products });
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.AboutUs;
using Transportation.Buisness.Services.ProductCategories;
using Transportation.Buisness.Services.ProductCategories.Dto;
using Transportation.Buisness.Services.Products;
using Transportation.Buisness.Services.Products.Dto;

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

        public async Task<IActionResult> Index()
        {
            var categoriesResult = await _productCategoryService.List(new ProductCategoryListRequestDto());
            var categories = categoriesResult.IsSucceeded
                ? categoriesResult.Data
                : new SPFOutPutDto<ProductCategoryListResponseDto>();

            return View(categories);
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

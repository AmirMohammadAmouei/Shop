using Microsoft.AspNetCore.Mvc;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.ProductCategories;
using Transportation.Buisness.Services.ProductCategories.Dto;
using Transportation.Buisness.Services.Products;
using Transportation.Buisness.Services.Products.Dto;

namespace Transportation.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductCategoryService _productCategoryService;
        private readonly ProductService _productService;

        public ProductController(ProductCategoryService productCategoryService, ProductService productService)
        {
            _productCategoryService = productCategoryService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categoryResponse = await _productCategoryService.List(new ProductCategoryListRequestDto());

            var categories = categoryResponse.IsSucceeded ? categoryResponse.Data : new SPFOutPutDto<ProductCategoryListResponseDto>();

            return View(categories);
        }


        [HttpGet]
        public async Task<IActionResult> GetProductsByCategoryId(long categoryId, ProductListRequestDto request)
        {

            SPFOutPutDto<ProductListResponseDto> data;

            if (categoryId == 0)
            {
                var result = await _productService.List(request);
                data = result.IsSucceeded ? result.Data : new SPFOutPutDto<ProductListResponseDto>();
            }
            else
            {
                var result = await _productService.GetProductsByCategoryId(categoryId, request);
                data = result.IsSucceeded ? result.Data : new SPFOutPutDto<ProductListResponseDto>();
            }
            return Json(new
            {
                isSucceded = true,
                items = data.Items,
                totalPages = data.TotalPages,
                currentPage = data.PageNumber,
                totalCount = data.TotalCount
            });

        }
    }
}

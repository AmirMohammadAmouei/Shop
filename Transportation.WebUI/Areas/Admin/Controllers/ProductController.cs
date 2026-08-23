using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transportation.Buisness._0.Common.Constants;
using Transportation.Buisness._0.Common.FileManager;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.ProductCategories;
using Transportation.Buisness.Services.ProductCategories.Dto;
using Transportation.Buisness.Services.Products;
using Transportation.Buisness.Services.Products.Dto;

namespace Transportation.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly ProductCategoryService _categoryService;
        private readonly IUploadFileService _uploadFileService;
        private readonly IWebHostEnvironment _env;

        public ProductController(
            ProductService productService,
            ProductCategoryService categoryService,
            IUploadFileService uploadFileService,
            IWebHostEnvironment env)
        {
            _productService = productService;
            _categoryService = categoryService;
            _uploadFileService = uploadFileService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var productsResult = await _productService.List(new ProductListRequestDto());
            var products = productsResult.IsSucceeded
                ? productsResult.Data
                : new SPFOutPutDto<ProductListResponseDto>();

            var categories = await _categoryService.List(new ProductCategoryListRequestDto());
            ViewBag.Categories = categories.Data?.Items ?? new List<ProductCategoryListResponseDto>();

            return View(products);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault());


            var dto = new CreateProductDto
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ShowPrice = request.ShowPrice,
                ProductCategoryId = request.ProductCategoryId,
                Images = request.Images
            };

            var result = await _productService.Create(dto);

            if (!result.IsSucceeded)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateProductRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault());

            var dto = new UpdateProductDto
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ShowPrice = request.ShowPrice,
                ProductCategoryId = request.ProductCategoryId,
                NewImages = request.NewImages,
                DeletedImageIds = request.DeletedImageIds
            };

            var result = await _productService.Update(dto);

            if (!result.IsSucceeded)
                return BadRequest(result.Message);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _productService.Delete(id);

            if (!result.IsSucceeded)
                return BadRequest(result.Message);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> DownloadImage(long imageId)
        {
            var result = await _productService.GetImagePath(imageId);

            if (!result.IsSucceeded)
                return NotFound(result.Message);

            var download = _uploadFileService.Download(result.Data);

            if (!download.IsSucceeded)
                return NotFound(download.Message);

            return File(download.Bytes, download.ContentType, download.FileName);
        }
    }
}

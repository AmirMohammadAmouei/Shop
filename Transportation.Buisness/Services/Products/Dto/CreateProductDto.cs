using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Transportation.Buisness._0.Common.Paging;

namespace Transportation.Buisness.Services.Products.Dto
{
    public class CreateProductDto
    {
        public long ProductCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool ShowPrice { get; set; }
        public List<IFormFile> Images { get; set; } = new();
    }


    public class UpdateProductDto
    {
        public long Id { get; set; }
        public long ProductCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool ShowPrice { get; set; }
        public List<IFormFile> NewImages { get; set; } = new();
        public List<long> DeletedImageIds { get; set; } = new();
    }

    public class UpdateProductRequestDto
    {
        [Required]
        public long Id { get; set; }
        [Required(ErrorMessage = "نام محصول الزامی است")]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool ShowPrice { get; set; }
        [Required(ErrorMessage = "دسته‌بندی الزامی است")]
        public long ProductCategoryId { get; set; }
        public List<IFormFile> NewImages { get; set; } = new();
        public List<long> DeletedImageIds { get; set; } = new();
    }

    public class ProductListResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool ShowPrice { get; set; }
        public long ProductCategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductImageResponseDto> Images { get; set; } = new();
    }

    public class ProductImageResponseDto
    {
        public long Id { get; set; }
        public string Path { get; set; }
    }

    public class ProductListRequestDto : SPFInputDto
    {
    }

    public class ProductDetailsDto
    {
        public long ProductCategoryId { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool ShowPrice { get; set; }
        public List<ProductImageResponseDto> ImagesPath { get; set; } = new();
    }

   
}

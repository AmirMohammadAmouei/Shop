using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Transportation.Buisness.Services.Products.Dto
{
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

   
}

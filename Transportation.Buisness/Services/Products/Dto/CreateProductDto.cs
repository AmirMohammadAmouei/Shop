using Microsoft.AspNetCore.Http;

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

   
}

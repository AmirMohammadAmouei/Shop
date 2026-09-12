namespace Transportation.Buisness.Services.Products.Dto
{
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

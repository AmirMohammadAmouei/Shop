namespace Transportation.Buisness.Services.Products.Dto
{
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

   
}

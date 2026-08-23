using Transportation.Entities._0.Common;

namespace Transportation.Entities.Entities
{
    public class Product : Entity<long>
    {
        public Product()
        {
            ProductImages = new List<ProductImages>();
        }

        public long ProductCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool ShowPrice { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ICollection<ProductImages> ProductImages { get; set; }
    }
}

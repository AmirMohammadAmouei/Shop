using Transportation.Entities._0.Common;

namespace Transportation.Entities.Entities
{
    public class ProductCategory : Entity<long>
    {
        public ProductCategory()
        {
            Products = new List<Product>();
        }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}

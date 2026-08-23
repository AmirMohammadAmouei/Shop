using Transportation.Entities._0.Common;

namespace Transportation.Entities.Entities
{
    public class ProductImages : Entity<long>
    {
        public long ProductId { get; set; }
        public string Path { get; set; }
        public Product Product { get; set; }
    }
}

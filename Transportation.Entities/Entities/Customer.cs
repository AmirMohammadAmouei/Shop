using Transportation.Entities._0.Common;

namespace Transportation.Entities.Entities
{
    public class Customer :Entity<long>
    {
        public string Name { get; set; }
        public string LogoPath { get; set; }
    }
}

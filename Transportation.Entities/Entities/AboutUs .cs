using Transportation.Entities._0.Common;

namespace Transportation.Entities.Entities
{
    public class AboutUs : Entity<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string LogoPath { get; set; }
    }
}

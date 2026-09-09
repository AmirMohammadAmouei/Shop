using Transportation.Entities._0.Common;

namespace Transportation.Entities.Entities
{
    public class AboutUsGallery : Entity<long>
    {
        public long AboutUsId { get; set; }
        public AboutUs AboutUs { get; set; }
    }
}

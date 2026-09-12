using Transportation.Entities._0.Common;

namespace Transportation.Entities.Entities
{
    public class MobileAppCategory : Entity<long>
    {
        public MobileAppCategory()
        {
            Children = new List<MobileAppCategory>();
            MobileApps = new List<MobileApp>();
        }

        public string Name { get; set; }
        public long? ParentId { get; set; }
        public MobileAppCategory Parent { get; set; }
        public ICollection<MobileAppCategory> Children { get; set; }
        public ICollection<MobileApp> MobileApps { get; set; }
    }
}

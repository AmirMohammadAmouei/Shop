using Transportation.Entities._0.Common;

namespace Transportation.Entities.Entities
{
    public class CompanyFeature : Entity<long>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}

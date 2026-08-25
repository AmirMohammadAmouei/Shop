using Transportation.Entities._0.Common;

namespace Transportation.Entities.Entities
{
    public class MobileApp : Entity<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Platform { get; set; } // Android, iOS, Windows
        public string FilePath { get; set; }
        public string IconPath { get; set; }
        public long FileSize { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

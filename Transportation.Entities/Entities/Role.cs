using Microsoft.AspNetCore.Identity;
using Transportation.Entities._0.Common;

namespace Transportation.Entities.Entities
{
    public class Role : IdentityRole<long>, IEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdtedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public bool IsDeleted { get ; set ; }
    }
}

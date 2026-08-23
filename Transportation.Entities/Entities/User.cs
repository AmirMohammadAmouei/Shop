using Microsoft.AspNetCore.Identity;
using Transportation.Entities._0.Common;

namespace Transportation.Entities.Entities
{
    public class User : IdentityUser<long>, IEntity
    {
        public User()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdtedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public bool IsDeleted { get ; set ; }
    }
}

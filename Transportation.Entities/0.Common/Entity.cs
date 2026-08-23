namespace Transportation.Entities._0.Common
{
    public class Entity<TKey> : IEntity
    {
        public Entity()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public TKey Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdtedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

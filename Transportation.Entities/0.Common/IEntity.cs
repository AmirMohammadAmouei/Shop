namespace Transportation.Entities._0.Common
{
    public interface IEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdtedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}

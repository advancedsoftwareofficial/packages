namespace AdvancedSoftware.DataAccess.Entity
{
    public class EntityBase : IEntityBase
    {
        public int Id { get; set; }
        public byte[]? Timestamp { get; set; }
    }
}

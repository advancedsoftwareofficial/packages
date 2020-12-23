using System.ComponentModel.DataAnnotations;

namespace AdvancedSoftware.DataAccess.Entity
{
    public interface IEntityBase
    {
        int Id { get; set; }
        [Timestamp]
        byte[]? Timestamp { get; set; }
    }
}

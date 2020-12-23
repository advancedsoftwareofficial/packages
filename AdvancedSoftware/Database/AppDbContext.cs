using Microsoft.EntityFrameworkCore;

namespace AdvancedSoftware.DataAccess.Database
{
    public partial class AppDbContext<T> : DbContext where T : DbContext
    {
        public AppDbContext(DbContextOptions<T> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}

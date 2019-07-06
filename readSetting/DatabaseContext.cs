using Microsoft.EntityFrameworkCore;

namespace readSetting
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }
        public DbSet<OtherSet> OtherSet { get; set; }
    }
}

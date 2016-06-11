using ENBOrganizer.Domain.Entities;
using SQLite.CodeFirst;
using System.Data.Entity;

namespace ENBOrganizer.Domain.Data
{
    public class ENBOrganizerContext : DbContext
    {
        public ENBOrganizerContext() : base("database") { }

        public DbSet<Game> Games { get; set; }
        public DbSet<Binary> Binaries { get; set; }
        public DbSet<Preset> Presets { get; set; }
        public DbSet<MasterListItem> MasterListItem { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            SqliteCreateDatabaseIfNotExists<ENBOrganizerContext> sqlLiteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<ENBOrganizerContext>(modelBuilder);
            Database.SetInitializer(sqlLiteConnectionInitializer);
        }
    }
}

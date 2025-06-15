using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core._3D.Ef
{
    public class EventStoreContext : DbContext
    {
        public DbSet<EventModel> Events { get; set; }

        public EventStoreContext(DbContextOptions<EventStoreContext> options) : base(options)
        {
            Database.EnsureCreated();
            RelationalDatabaseCreator creator = (RelationalDatabaseCreator)Database.GetService<IDatabaseCreator>();
            try
            {
                creator.CreateTables();
            }
            catch (SqlException) { }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventModel>()
                .HasIndex(e => new { e.Correlation, e.SequenceNumber });

            base.OnModelCreating(modelBuilder);
        }
    }


    [Table("Events", Schema = "glmesh")]
    public class EventModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid Correlation { get; set; }

        [MaxLength(255)]
        public string EventType { get; set; } = string.Empty;

        public string EventDataJson { get; set; } = string.Empty;

        public int SequenceNumber { get; set; }
        public long Timestamp { get; set; }
    }
}
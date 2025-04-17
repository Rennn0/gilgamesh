using Hub.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hub.Database;

public class ApplicationContext : DbContext
{
    public DbSet<Client> Clients { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }
}

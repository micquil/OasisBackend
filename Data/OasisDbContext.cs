using Microsoft.EntityFrameworkCore;
using Oasis.Models;

public class OasisDbContext : DbContext
{
    public OasisDbContext(DbContextOptions<OasisDbContext> options) : base(options) { }

    // Add your database tables as DbSet properties
    public DbSet<AccountsPayable> AccountsPayables { get; set; }
    public DbSet<AccountsReceivable> AccountsReceivables { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Optional: Define additional configurations like relationships, constraints, etc.
    }
}

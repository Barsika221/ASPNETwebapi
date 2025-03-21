using Microsoft.EntityFrameworkCore;

public class MacAdressContext : DbContext
{
    public MacAdressContext(DbContextOptions<MacAdressContext> options) : base(options) { }
    public DbSet<MacAdress> MacAdresses { get; set; }
}
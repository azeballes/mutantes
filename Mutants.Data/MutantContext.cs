using Microsoft.EntityFrameworkCore;

namespace Mutants.Data
{
    public class MutantContext : DbContext
    {
        public MutantContext(DbContextOptions<MutantContext> options) : base(options) { }

        public DbSet<Adn> Adns { get; set; }
    }
}

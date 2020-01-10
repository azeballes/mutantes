using Microsoft.EntityFrameworkCore;

namespace Mutants.Data
{
    public class MutantContext : DbContext
    {
        public MutantContext(DbContextOptions options) : base(options) { }

        public DbSet<Dna> DnaSet { get; set; }
    }
}

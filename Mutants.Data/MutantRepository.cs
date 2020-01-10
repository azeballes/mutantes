using System.Linq;
using System.Text.Json;

namespace Mutants.Data
{
    public class MutantRepository : IMutantRepository
    {
        private readonly MutantContext _context;

        public MutantRepository(MutantContext context)
        {
            _context = context;
        }

        public void Save(string [] dna, bool mutant)
        {
            _context.DnaSet.Add(new Dna()
            {
                Mutant = mutant
                , Id =  JsonSerializer.Serialize(dna)
            });
            _context.SaveChanges();
        }

        public Dna FindByDna(string [] dna)
        {
            var key = JsonSerializer.Serialize(dna);
            return _context.DnaSet.Find(key);
        }

        public IQueryable<Dna> AsQueryable() => _context.DnaSet;

    }
}

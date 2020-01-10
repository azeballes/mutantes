using System.Linq;

namespace Mutants.Data
{
    public interface IMutantRepository
    {
        void Save(string [] dna, bool mutant);

        Dna FindByDna(string [] dna);

        IQueryable<Dna> AsQueryable();
    }
}
using System.Collections.Generic;
using System.Linq;
using Mutants.Data;

namespace Mutants.Model
{
    public class MutantRepository
    {
        private readonly MutantContext _context;

        public MutantRepository(MutantContext context)
        {
            _context = context;
        }

        public MutantStats Stats()
        {
            var result = from adns in _context.Adns
                group adns by adns.Mutant
                into m
                select new
                {
                    mutant = m.Key
                    , quantity = m.Count()
                };
            var stats = result.ToList();
            
            if (stats.Count == 0)
                return new MutantStats();

            return new MutantStats(stats.FirstOrDefault(r => r.mutant)?.quantity ?? 0L
                ,stats.FirstOrDefault(r => !r.mutant)?.quantity ?? 0L);
        }

        public void Save(string adn, bool mutant)
        {
            _context.Adns.Add(new Adn()
            {
                Mutant = mutant
                , Value = adn
            });
            _context.SaveChanges();
        }
    }
}

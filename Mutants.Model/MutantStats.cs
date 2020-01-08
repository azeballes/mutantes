namespace Mutants.Model
{
    public class MutantStats
    {
        public MutantStats() { }
        public MutantStats(long mutants, long humans)
        {
            Mutants = mutants;
            Humans = humans;
        }
        public long Mutants { get; }
        public long Humans { get; }

        public decimal Ratio => Humans == 0 ? Mutants == 0 ? 0m : 1m : (decimal) Mutants / Humans;
    }
}

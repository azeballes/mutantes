using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using Mutants.Data;
using Mutants.Model;
using Xunit;

namespace Mutants.Testing.Integration
{
    public class MutantTest : IClassFixture<MutantContextFixture>
    {
        private readonly MutantContextFixture _context;
        private readonly Mutant _sut;
        private readonly MutantRepository _mutantRepository;

        public MutantTest(MutantContextFixture context)
        {
            _context = context;
            _mutantRepository = new MutantRepository(_context.Context);
            _sut = new Mutant(_mutantRepository);
            DeleteAllAdns();
        }

        private void DeleteAllAdns()
        {
            _context.Context.RemoveRange( _context.Context.DnaSet );
            _context.Context.SaveChanges();
        }

        [Fact]
        public void InitiallyStatsAreZero()
        {   
            var stats = _sut.Stats();
            Assert.Equal( 0L, stats.Mutants);
            Assert.Equal(0L, stats.Humans);
            Assert.Equal(0.00m, stats.Ratio);
        }

        [Fact]
        public void StatsWithOneHumanShouldSuccess()
        {
            CreateAdnInRepository(false);

            var stats = _sut.Stats();
            Assert.Equal(0L, stats.Mutants);
            Assert.Equal(1L, stats.Humans);
            Assert.Equal(0.00m, stats.Ratio);
        }

        private void CreateAdnInRepository(bool mutant)
        {
            _mutantRepository.Save(AnyDna(), mutant);
        }

        private string[] AnyDna()
        {
            var generator = new RandomGenerator();

            return Enumerable.Range(1, 10).Select(i => generator.Phrase(10)).ToArray();
        }

        [Fact]
        public void StatsWithOneMutantShouldSuccess()
        {
            CreateAdnInRepository(true);

            var stats = _sut.Stats();
            Assert.Equal(1L, stats.Mutants);
            Assert.Equal(0L, stats.Humans);
            Assert.Equal(1.00m, stats.Ratio);
        }

        [Fact]
        public void StatsWithOneMutantAndOneHumanShouldSuccess()
        {
            CreateAdnInRepository(true);
            CreateAdnInRepository(false);

            var stats = _sut.Stats();
            Assert.Equal(1L, stats.Mutants);
            Assert.Equal(1L, stats.Humans);
            Assert.Equal(1.00m, stats.Ratio);
        }

        [Fact]
        public void StatsWithFourtyMutantAndOneHundredHumansShouldSuccess()
        {   
            foreach (var n in Enumerable.Range(1, 100))
            {
                if (n <= 40)
                    CreateAdnInRepository(true);
                CreateAdnInRepository(false);
            }

            var stats = _sut.Stats();
            Assert.Equal(40L, stats.Mutants);
            Assert.Equal(100L, stats.Humans);
            Assert.Equal(0.40m, stats.Ratio);
        }
    }
}

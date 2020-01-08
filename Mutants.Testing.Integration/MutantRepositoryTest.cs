using System.Linq;
using Mutants.Model;
using Xunit;

namespace Mutants.Testing.Integration
{
    public class MutantRepositoryTest : IClassFixture<MutantContextFixture>
    {
        private readonly MutantContextFixture _context;
        private readonly MutantRepository _sut;

        public MutantRepositoryTest(MutantContextFixture context)
        {
            _context = context;
            _sut = new MutantRepository(context.Context);
            DeleteAllAdns();
        }

        private void DeleteAllAdns()
        {
            _context.Context.RemoveRange( _context.Context.Adns );
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

        private void CreateAdnInRepository(bool mutant) => _sut.Save("{\"adn\":[\"A\",\"T\"]}", mutant);

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

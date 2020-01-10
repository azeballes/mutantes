using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Mutants.Data;
using Xunit;
using Mutants.Model;
using Newtonsoft.Json;

namespace Mutants.Testing.Unit
{
    public class MutantTest
    {
        private readonly Mutant _sut;
        private Mock<IMutantRepository> _mutantRepositoryMock;
        private IList<Dna> _dnaList;

        public MutantTest(){
            _mutantRepositoryMock = new Mock<IMutantRepository>();
            _sut = new Mutant(_mutantRepositoryMock.Object);
            _dnaList = new List<Dna>();
        }
        
        [Fact]
        public void IsMutantWithOutDnaShouldFail()
        {
            var exception = Assert.Throws<Exception>( () => _sut.IsMutant(null) );
            Assert.Equal( Mutant.MustBeIndicateDnaMessage, exception.Message );
            _mutantRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void IsMutantWithEmptyDnaShouldFail()
        {
            var exception = Assert.Throws<Exception>(() => _sut.IsMutant(new string[] { }));
            Assert.Equal(Mutant.MustBeIndicateDnaMessage, exception.Message);
            _mutantRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void IsMutantWithOutNxNDnaMatrixShouldFail()
        {
            string[] notNxNMutantDna = { "ATGC", "CAGC", "CGTC", "CGTC", "CGTC" };
            var exception = Assert.Throws<Exception>(() => _sut.IsMutant(notNxNMutantDna));
            _mutantRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void IsMutantWithDnaMatrixLessThanFourDimensionsShouldBeFalse()
        {
            string[] dnaArrayLessThanFourCharacters = { "AAA", "AAA", "AAA" };
            NotExistsDna(dnaArrayLessThanFourCharacters);
            var result = _sut.IsMutant(dnaArrayLessThanFourCharacters);
            
            Assert.False(result);
            VerifyInteractionWithRepositoryWhenNoExistsDna(dnaArrayLessThanFourCharacters);
        }

        private void NotExistsDna(string[] dnaArrayLessThanFourCharacters)
        {
            _mutantRepositoryMock
                .Setup(m => m.FindByDna(dnaArrayLessThanFourCharacters))
                .Returns<Dna>(null);
        }

        [Fact]
        public void IsMutantWithNoValidCharInDnaShouldFail()
        {
            string[] noValidCharInDna = { "ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCXCTA", "TCACTG" };
            var exception = Assert.Throws<Exception>(() => _sut.IsMutant(noValidCharInDna));
            Assert.Equal(Mutant.ValidDnaCharsMessage, exception.Message);
            _mutantRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void IsMutantWithValidNoMutantDnaShouldBeFalse()
        {
            string [] validNoMutantDna = { "ATGC", "ATGC", "ATGC", "GCAT" };
            NotExistsDna(validNoMutantDna);
            var result = _sut.IsMutant(validNoMutantDna);
            Assert.False(result);
            VerifyInteractionWithRepositoryWhenNoExistsDna(validNoMutantDna);
        }

        private void VerifyInteractionWithRepositoryWhenNoExistsDna(string[] validDna)
            => VerifyInteractionWithRepository(validDna, Times.Once());

        [Fact]
        public void IsMutantWithOneValidMutantDnaShouldBeFalse()
        {
            string[] validNoMutantDna = { "ATGC", "ATGC", "ATGC", "GCAT" };
            var result = _sut.IsMutant(validNoMutantDna);
            Assert.False(result);
        }

        [Fact]
        public void IsMutantWithTwoDnaValidSequencesByRowShouldSuccess()
        {
            var validMutantDna = new [] {"AAAA", "ATCG", "AAAA", "CTGG"};
            var result = _sut.IsMutant(validMutantDna);
            Assert.True(result);
        }

        [Fact]
        public void IsMutantWithTwoDnaValidSequencesByRowAndColumnShouldSuccess()
        {
            var validMutantDna = new[] { "AAAAA", "ATCGA", "GGCCA", "CTGGA", "CTGGA" };
            var result = _sut.IsMutant(validMutantDna);
            Assert.True(result);
        }

        [Fact]
        public void IsMutantWithTwoDnaValidSequencesByObliqueLeftToRightShouldSuccess()
        {
            var validMutantDna = new[] { "ACGTA", "ATCGT", "GACCA", "CTAGC", "CTGAA" };
            var result = _sut.IsMutant(validMutantDna);
            Assert.True(result);
        }

        [Fact]
        public void IsMutantWithTwoDnaValidSequencesByObliqueRightToLeftShouldSuccess()
        {
            var validMutantDna = new[] { "ACGTA", "ACTGT", "GTCTA", "TTTGT", "CTGAA" };
            var result = _sut.IsMutant(validMutantDna);
            Assert.True(result);
        }

        [Fact]
        public void IsMutantWithThreeEqualsCharInSequenceShouldBeFalse()
        {
            var validMutantDna = new[] { "AAACC", "CCCGG", "GGGTT", "TTTAA", "AAACC" };
            var result = _sut.IsMutant(validMutantDna);
            Assert.False(result);
        }

        [Fact]
        public void IsMutantShouldBeFalse()
        {
            var validNotMutantDna = new[] { "ATGCGA", "CAGTGC", "TTATTT", "AGACGG", "GCGTCA", "TCACTG" };
            var result = _sut.IsMutant(validNotMutantDna);
            Assert.False(result);
        }

        [Fact]
        public void IsMutantForExistingNotMutantDnaShouldBeFalse()
        {
            var validNotMutantDna = new[] { "ATGCGA", "CAGTGC", "TTATTT", "AGACGG", "GCGTCA", "TCACTG" };
            _mutantRepositoryMock
                .Setup(m => m.FindByDna(validNotMutantDna))
                .Returns(ValidAdn(validNotMutantDna, false));
            var result = _sut.IsMutant(validNotMutantDna);
            Assert.False(result);
            VerifyInteractionWithRepository(validNotMutantDna, Times.Never());
        }

        [Fact]
        public void IsMutantForExistingMutantDnaShouldBeTrue()
        {
            var mutantDna = new[] { "ACGTA", "ACTGT", "GTCTA", "TTTGT", "CTGAA" };
            _mutantRepositoryMock
                .Setup(m => m.FindByDna(mutantDna))
                .Returns(ValidAdn(mutantDna, true));
            var result = _sut.IsMutant(mutantDna);
            Assert.True(result);
            VerifyInteractionWithRepository(mutantDna, Times.Never());
        }

        private void VerifyInteractionWithRepository(string[] validDna, Times saveTimes)
        {
            _mutantRepositoryMock.Verify(m => m.FindByDna(validDna), Times.Once);
            _mutantRepositoryMock.Verify(m => m.Save(validDna, false), saveTimes);
        }

        private static Dna ValidAdn(IEnumerable dna, bool isMutant)
        {
            return new Dna()
            {
                Id = JsonConvert.SerializeObject(dna)
                , Mutant = isMutant
            };
        }

        //--------------------------------------------------------

        [Fact]
        public void StatsInitiallyAreZero()
        {
            var stats = _sut.Stats();
            Assert.Equal(0L, stats.Mutants);
            Assert.Equal(0L, stats.Humans);
            Assert.Equal(0.00m, stats.Ratio);
        }

        [Fact]
        public void StatsWithOneHumanShouldSuccess()
        {
            AddAdnInRepository(false);

            var stats = _sut.Stats();
            Assert.Equal(0L, stats.Mutants);
            Assert.Equal(1L, stats.Humans);
            Assert.Equal(0.00m, stats.Ratio);
        }
        private void AddAdnInRepository(bool mutant)
        {
            var anyDna = new[] {"A", "G"};
            _dnaList.Add(ValidAdn(anyDna, mutant));
            _mutantRepositoryMock.Setup(m => m.AsQueryable())
                .Returns(_dnaList.AsQueryable());
        }

        [Fact]
        public void StatsWithOneMutantShouldSuccess()
        {
            AddAdnInRepository(true);

            var stats = _sut.Stats();
            Assert.Equal(1L, stats.Mutants);
            Assert.Equal(0L, stats.Humans);
            Assert.Equal(1.00m, stats.Ratio);
        }

        [Fact]
        public void StatsWithOneMutantAndOneHumanShouldSuccess()
        {
            AddAdnInRepository(true);
            AddAdnInRepository(false);

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
                    AddAdnInRepository(true);
                AddAdnInRepository(false);
            }

            var stats = _sut.Stats();
            Assert.Equal(40L, stats.Mutants);
            Assert.Equal(100L, stats.Humans);
            Assert.Equal(0.40m, stats.Ratio);
        }
        
    }
}

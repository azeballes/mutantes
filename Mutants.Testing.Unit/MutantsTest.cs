using System;
using Xunit;
using Mutants.Model;

namespace Mutants.Testing.Unit
{
    public class MutantsTest
    {
        private readonly Mutant _sut;
        
        public MutantsTest(){
            _sut = new Mutant();
        }
        
        [Fact]
        public void IsMutantWithOutDnaShouldFail()
        {
            var exception = Assert.Throws<Exception>( () => _sut.IsMutant(null) );
            Assert.Equal( Mutant.MustBeIndicateDnaMessage, exception.Message );
        }

        [Fact]
        public void IsMutantWithEmptyDnaShouldFail()
        {
            var exception = Assert.Throws<Exception>(() => _sut.IsMutant(new string[] { }));
            Assert.Equal(Mutant.MustBeIndicateDnaMessage, exception.Message);
        }

        [Fact]
        public void IsMutantWithOutNxNDnaMatrixShouldFail()
        {
            string[] notNxNMutantDna = { "ATGC", "CAGC", "CGTC", "CGTC", "CGTC" };
            var exception = Assert.Throws<Exception>(() => _sut.IsMutant(notNxNMutantDna));
            Assert.Equal(Mutant.DnaChainMustBeNxNMessage, exception.Message);
        }

        [Fact]
        public void IsMutantWithDnaMatrixLessThanFourDimensionsShouldBeFalse()
        {
            string[] dnaArrayLessThanFourCharacters = { "AAA", "AAA", "AAA" };
            var result = _sut.IsMutant(dnaArrayLessThanFourCharacters);
            Assert.False(result);
        }

        [Fact]
        public void IsMutantWithNoValidCharInDnaShouldFail()
        {
            string[] noValidCharInDna = { "ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCXCTA", "TCACTG" };
            var exception = Assert.Throws<Exception>(() => _sut.IsMutant(noValidCharInDna));
            Assert.Equal(Mutant.ValidDnaCharsMessage, exception.Message);
        }

        [Fact]
        public void IsMutantWithValidNoMutantDnaShouldBeFalse()
        {
            string [] validNoMutantDna = { "ATGC", "ATGC", "ATGC", "GCAT" };
            var result = _sut.IsMutant(validNoMutantDna);
            Assert.False(result);
        }

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
            var validMutantDna = new[] { "ATGCGA", "CAGTGC", "TTATTT", "AGACGG", "GCGTCA", "TCACTG" };
            var result = _sut.IsMutant(validMutantDna);
            Assert.False(result);
        }
    }
}

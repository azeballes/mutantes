﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Mutants.Model
{
    public class Mutant
    {
        private const int NumberOfConsecutiveChars = 4;
        private const int MinNumberOfSequencesToBeMutant = 2;
        public static readonly string MustBeIndicateDnaMessage = "Debe indicar una cadena de ADN";
        public static readonly string DnaChainMustBeNxNMessage = "La cadena de ADN debe ser una matriz de N x N";
        public static readonly string ValidDnaCharsMessage = "La cadena de ADN sólo debe contener los caracteres (A,T,C,G)";
        public static char[] ValidChars = {'A','C','G','T'};

        public bool IsMutant(string [] dna)
        {
            return ValidateDna(dna) && AllSequencesFound(dna) > 1;
        }

        private static bool ValidateDna(IReadOnlyCollection<string> dna)
        {
            if  (dna == null || dna.Count == 0)
                throw new Exception(MustBeIndicateDnaMessage);
            
            if (!IsMatrix(dna))
                throw new Exception(DnaChainMustBeNxNMessage);
            
            if (!OnlyContainsChars( dna, ValidChars))
                throw new Exception(ValidDnaCharsMessage);

            return dna.Count >= NumberOfConsecutiveChars;
        }

        private static int NumberOfSequencesInColumns(IReadOnlyCollection<string> matrix)
        {
            return matrix.Select((t, x) => LineContainsConsecutiveChars(matrix, x, 0, (i, j) => j < matrix.Count, 0)).Sum();
        }

        private static int NumberOfSequencesInRows(IReadOnlyCollection<string> matrix)
        {
            return matrix.Select((t, y) => LineContainsConsecutiveChars(matrix, 0, y, (i, j) => i < matrix.Count, 1, incrementY:0)).Sum();
        }

        public int AllSequencesFound(IReadOnlyCollection<string> matrix)
        {
            return NumberOfSequencesInRows(matrix)
                + NumberOfSequencesInColumns(matrix) 
                + NumberOfSequencesInLeftToRightObliqueLines(matrix) 
                + NumberOfSequencesInRightToLeftObliqueLines(matrix);
        }

        private static bool IsMatrix(IReadOnlyCollection<string> dna) => dna.All(s => s.Length == dna.Count);

        private static bool OnlyContainsChars(IReadOnlyCollection<string> matrix, IEnumerable<char> chars)
        {
            return matrix.All(row => row.All(c => chars.Any(validChar => validChar == c)));
        }

        private static int NumberOfSequencesInLeftToRightObliqueLines(IReadOnlyCollection<string> matrix)
        {
            var numberOfSequences = 0;
            for (var offset = 0; matrix.Count - offset >= NumberOfConsecutiveChars && NotMutantYet(numberOfSequences); offset++)
            {
                numberOfSequences += ObliqueLineFromLeftToRight(matrix, offset, 0);
                if (NotMutantYet(numberOfSequences) && offset > 0)
                    numberOfSequences += ObliqueLineFromLeftToRight(matrix, 0, offset);
            }
            return numberOfSequences;
        }

        private static bool NotMutantYet(int numberOfSequences) => MinNumberOfSequencesToBeMutant > numberOfSequences;

        private static int NumberOfSequencesInRightToLeftObliqueLines(IReadOnlyCollection<string> matrix)
        {
            var numberOfSequences = 0;
            var initialColumn = matrix.Count - 1;
            for (var offset = initialColumn; offset >= 3 && NotMutantYet(numberOfSequences); offset--)
            {
                numberOfSequences += ObliqueLineFromRightToLeft(matrix, offset, 0);
                if (NotMutantYet(numberOfSequences) && offset < initialColumn)
                    numberOfSequences += ObliqueLineFromRightToLeft(matrix, initialColumn, initialColumn - offset);
            }
            return numberOfSequences;
        }

        private static int ObliqueLineFromLeftToRight(IReadOnlyCollection<string> matrix, int initialX, int initialY)
        {
            return LineContainsConsecutiveChars(matrix, initialX, initialY, (i, j) => i < matrix.Count && j < matrix.Count, 1);
        }

        private static int ObliqueLineFromRightToLeft(IReadOnlyCollection<string> matrix, int initialX, int initialY)
        {
            return LineContainsConsecutiveChars(matrix, initialX, initialY, (i, j) => i >= 0 && j < matrix.Count, -1);
        }

        private static int LineContainsConsecutiveChars(IReadOnlyCollection<string> matrix, int initialX, int initialY, Func<int, int, bool> checkLimitCondition, int incrementX, int incrementY = 1)
        {
            var previousChar = char.MinValue;
            var consecutiveEqualChars = 1;
            var y = initialY;
            for (var x = initialX; checkLimitCondition(x, y) && consecutiveEqualChars < NumberOfConsecutiveChars; x += incrementX)
            {
                if (previousChar == matrix.ElementAt(y)[x])
                    consecutiveEqualChars++;
                else
                    consecutiveEqualChars = 1;
                previousChar = matrix.ElementAt(y)[x];
                y += incrementY;
            }
            return consecutiveEqualChars == NumberOfConsecutiveChars ? 1 : 0;
        }
    }
}
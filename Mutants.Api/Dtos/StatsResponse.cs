using Mutants.Model;
using System.Text.Json.Serialization;

namespace Mutants.Api.Dtos
{
    public class StatsResponse
    {
        public StatsResponse(MutantStats mutantStats)
        {
            Mutants = mutantStats.Mutants;
            Humans = mutantStats.Humans;
            Ratio = mutantStats.Ratio;
        }

        [JsonPropertyName("count_mutant_dna")]
        public long Mutants { get; }
        [JsonPropertyName("count_human_dna")]
        public long Humans { get; }
        [JsonPropertyName("ratio")]
        public decimal Ratio { get; }
    }
}

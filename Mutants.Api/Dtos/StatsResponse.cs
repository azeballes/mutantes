using System.Text.Json.Serialization;

namespace Mutants.Api.Dtos
{
    public class StatsResponse
    {
        [JsonPropertyName("count_mutant_dna")]
        public long Mutants { get; }
        [JsonPropertyName("count_human_dna")]
        public long Humans { get; }
        [JsonPropertyName("ratio")]
        public decimal Ratio { get; }
    }
}

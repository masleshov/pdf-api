using System.Text.Json.Serialization;

namespace Pdf.Api.External.Dto
{
    internal sealed record ConvertFromEmailResponseDto
    {
        [JsonPropertyName("jobId")]
        public string JobId { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }
        
        [JsonPropertyName("pageCount")]
        public int PageCount { get; init; }

        [JsonPropertyName("error")]
        public bool Error { get; init; }

        [JsonPropertyName("status")]
        public int Status { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("remainingCredits")]
        public int RemainingCredits { get; init; }
    }
}
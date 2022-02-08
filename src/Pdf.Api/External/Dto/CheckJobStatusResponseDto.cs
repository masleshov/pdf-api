using System.Text.Json.Serialization;

namespace Pdf.Api.External.Dto
{
    internal sealed record CheckJobStatusResponseDto
    {
        [JsonPropertyName("status")]
        public CheckJobStatus Status { get; init; }

        [JsonPropertyName("remainingCredits")]
        public int RemainingCredits { get; init; }
    }
}
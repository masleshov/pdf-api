using System.Text.Json.Serialization;

namespace Pdf.Api.External.Dto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum CheckJobStatus
    {
        [JsonPropertyName("working")]
        Working,
        [JsonPropertyName("success")]
        Success,
        [JsonPropertyName("failed")]
        Failed,
        [JsonPropertyName("aborted")]
        Aborted,
        [JsonPropertyName("unknown")]
        Unknown
    }
}
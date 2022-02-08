using System.Text.Json.Serialization;

namespace Pdf.Api.External.Dto
{
    internal sealed record TranslateRequestDto
    {
        [JsonPropertyName("url")]
        public string SourceFileUrl { get; init; }

        [JsonPropertyName("name")]
        public string OutputFileName { get; init; }

        [JsonPropertyName("langFrom")]
        public Language LangFrom { get; init; }

        [JsonPropertyName("langTo")]
        public Language LangTo { get; init; }

        [JsonPropertyName("async")]
        public bool Async { get; init; }

        public TranslateRequestDto(string sourceFileUrl, string outputFileName, Language from, Language to)
        {
            SourceFileUrl = sourceFileUrl;
            OutputFileName = outputFileName;
            LangFrom = from;
            LangTo = to;
        }
    }
}
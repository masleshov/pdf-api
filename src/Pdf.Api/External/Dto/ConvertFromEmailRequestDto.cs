using System.Text.Json.Serialization;

namespace Pdf.Api.External.Dto
{
    internal sealed record ConvertFromEmailRequestDto
    {
        [JsonPropertyName("url")]
        public string SourceFileUrl { get; init; }

        [JsonPropertyName("embedAttachments")]
        public bool EmbedAttachments { get; init; }

        [JsonPropertyName("convertAttachments")]
        public bool ConvertAttachments { get; init; }

        [JsonPropertyName("paperSize")]
        public PaperSize PaperSize { get; init; }

        [JsonPropertyName("name")]
        public string OutputFileName { get; init; }

        [JsonPropertyName("async")]
        public bool Async { get; init; }

        public ConvertFromEmailRequestDto(string sourceFileUrl, bool embedAttachments, bool convertAttachments, string outputFileName)
        {
            SourceFileUrl = sourceFileUrl;
            EmbedAttachments = embedAttachments;
            ConvertAttachments = convertAttachments;
            PaperSize = PaperSize.Letter;
            OutputFileName = outputFileName;
            Async = false;
        }
    }
}
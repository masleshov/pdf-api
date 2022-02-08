using System.Text.Json.Serialization;

namespace Pdf.Api.External.Dto
{
    internal sealed record ConvertFromWebPageRequestDto
    {
        [JsonPropertyName("url")]
        public string WebPageUrl { get; init; }

        [JsonPropertyName("margins")]
        public string Margins { get; init; }

        [JsonPropertyName("paperSize")]
        public PaperSize PaperSize { get; init; }

        [JsonPropertyName("orientation")]
        public Orientation Orientation { get; init; }

        [JsonPropertyName("printBackground")]
        public bool PrintBackground { get; init; }

        [JsonPropertyName("header")]
        public string Header { get; init; }

        [JsonPropertyName("footer")]
        public string Footer { get; init; }

        [JsonPropertyName("async")]
        public bool Async { get; init; }

        [JsonPropertyName("profiles")]
        public string Profiles { get; init; }

        public ConvertFromWebPageRequestDto(string webPageUrl)
        {
            WebPageUrl = webPageUrl;
            Margins = "5mm";
            PaperSize = PaperSize.Letter;
            Orientation = Orientation.Portrait;
            PrintBackground = true;
            Header = string.Empty;
            Footer = string.Empty;
            Async = false;
            Profiles = string.Empty;
        }
    }
}
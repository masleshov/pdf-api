using System.Text.Json.Serialization;

namespace Pdf.Api.External.Dto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum PaperSize
    {
        Letter,
        A4,
        A5,
        A6
    }
}
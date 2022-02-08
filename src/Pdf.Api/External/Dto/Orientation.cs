using System.Text.Json.Serialization;

namespace Pdf.Api.External.Dto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum Orientation
    {
        Portrait,
        Landscape
    }
}
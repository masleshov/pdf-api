namespace Pdf.Api.Configuration
{
    public sealed record PdfApiConfiguration
    {
        public string Uri { get; init; }
        public string ApiKey { get; init; }
    }
}
namespace Pdf.Api.Dto.v1
{
    public sealed record WebPageToPdfFromSourceRequestDto
    {
        public string WebPageUrl { get; init; }
    }
}
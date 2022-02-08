namespace Pdf.Api.Dto.v1
{
    public sealed record EmailToPdfFromSourceRequestDto
    {
        public string SourceFileUrl { get; init; }
        public string OutputFileName { get; init; }
    }
}
using Pdf.Api.External.Dto;

namespace Pdf.Api.Dto.v1
{
    public sealed record TranslateRequestDto
    {
        public string SourceFileUrl { get; init; }
        public string OutputFileName { get; init; }
        public Language LangFrom { get; init; }
        public Language LangTo { get; init; }
    }
}
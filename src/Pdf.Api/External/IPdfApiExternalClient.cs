using System.Threading.Tasks;
using Pdf.Api.External.Dto;
using Refit;

namespace Pdf.Api.External
{
    internal interface IPdfApiExternalClient
    {
        [Post("/v1/pdf/convert/from/email")]
        Task<ConvertResponseDto> ConvertEmailMessageFromSource([Body] ConvertFromEmailRequestDto request);

        [Post("/v1/pdf/convert/from/url")]
        Task<ConvertResponseDto> ConvertWebPageFromSource([Body] ConvertFromWebPageRequestDto request);

        [Post("/v1/pdf/translate")]
        Task<ConvertResponseDto> Translate([Body] TranslateRequestDto request);

        [Get("/v1/job/check")]
        Task<CheckJobStatusResponseDto> CheckJob([Query, AliasAs("jobId")] string jobId);
    }
}
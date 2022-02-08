using System.Threading.Tasks;
using Pdf.Api.Exception;
using Pdf.Api.External;
using Pdf.Api.External.Dto;
using Pdf.Api.Infrastructure;

namespace Pdf.Api.Service
{
    public interface IEmailToPdfConvertService
    {
        Task<string> Convert(string sourceFileUrl, string outputFileName);
    }

    internal sealed class EmailToPdfConvertService : IEmailToPdfConvertService
    {
        private readonly JobStatusChecker _jobStatusChecker;
        private readonly IPdfApiExternalClient _pdfApiExternalClient;

        public EmailToPdfConvertService(JobStatusChecker jobStatusChecker, IPdfApiExternalClient pdfApiExternalClient)
        {
            _pdfApiExternalClient = pdfApiExternalClient;
            _jobStatusChecker = jobStatusChecker;
        }

        public async Task<string> Convert(string sourceFileUrl, string outputFileName)
        {
            var response = await _pdfApiExternalClient.ConvertEmailMessageFromSource(new ConvertFromEmailRequestDto(
                sourceFileUrl: sourceFileUrl,
                embedAttachments: true,
                convertAttachments: true,
                outputFileName: outputFileName
            ));

            if (response.Error) throw new PdfApiException($"External api respond with status {response.Status}");

            return response.Url;
        }
    }
}
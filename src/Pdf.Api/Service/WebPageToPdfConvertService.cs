using System.Threading.Tasks;
using Pdf.Api.Exception;
using Pdf.Api.External;
using Pdf.Api.External.Dto;

namespace Pdf.Api.Service
{
    public interface IWebPageToPdfConvertService
    {
        Task<string> Convert(string webPageUrl);
    }

    internal sealed class WebPageToPdfConvertService : IWebPageToPdfConvertService
    {
        private readonly IPdfApiExternalClient _pdfApiExternalClient;

        public WebPageToPdfConvertService(IPdfApiExternalClient pdfApiExternalClient)
        {
            _pdfApiExternalClient = pdfApiExternalClient;
        }

        public async Task<string> Convert(string webPageUrl)
        {
            var response = await _pdfApiExternalClient.ConvertWebPageFromSource(new ConvertFromWebPageRequestDto(webPageUrl));

            if (response.Error) throw new PdfApiException($"External api respond with status {response.Status}");

            return response.Url;
        }
    }
}
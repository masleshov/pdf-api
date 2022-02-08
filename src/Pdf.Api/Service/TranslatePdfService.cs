using System.Threading.Tasks;
using Pdf.Api.Exception;
using Pdf.Api.External;
using Pdf.Api.External.Dto;

namespace Pdf.Api.Service
{
    public interface ITranslatePdfService
    {
        Task<string> Translate(string sourceFileUrl, string outputFileName, Language from, Language to);
    }

    internal sealed class TranslatePdfService : ITranslatePdfService
    {
        private readonly IPdfApiExternalClient _pdfApiExternalClient;

        public TranslatePdfService(IPdfApiExternalClient pdfApiExternalClient)
        {
            _pdfApiExternalClient = pdfApiExternalClient;
        }

        public async Task<string> Translate(string sourceFileUrl, string outputFileName, Language from, Language to)
        {
            try 
            {
                var response = await _pdfApiExternalClient.Translate(new TranslateRequestDto(
                    sourceFileUrl: sourceFileUrl,
                    outputFileName: outputFileName,
                    from: from,
                    to: to
                ));

                if (response.Error) throw new PdfApiException($"External api respond with status {response.Status}");

                return response.Url;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
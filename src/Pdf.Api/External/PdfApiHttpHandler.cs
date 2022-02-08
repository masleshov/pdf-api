using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pdf.Api.Configuration;

namespace Pdf.Api.External
{
    public sealed class PdfApiHttpHandler : DelegatingHandler
    {
        private readonly PdfApiConfiguration _configuration;

        public PdfApiHttpHandler(IOptions<PdfApiConfiguration> configuration) => _configuration = configuration.Value;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            requestMessage.Headers.Add("x-api-key", _configuration.ApiKey);
            return await base.SendAsync(requestMessage, cancellationToken);
        }
    }
}
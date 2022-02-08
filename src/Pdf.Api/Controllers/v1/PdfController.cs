using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pdf.Api.Dto.v1;
using Pdf.Api.Service;

namespace Pdf.Api.Controllers.v1
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion(Infrastructure.Constant.ApiVersion.V1)]
    [Route("/api/v{version:apiVersion}/pdf")]
    public sealed class PdfController : ControllerBase
    {
        private readonly IEmailToPdfConvertService _emailToPdfConvertService;
        private readonly IWebPageToPdfConvertService _webPageToPdfConvertService;
        private readonly ITranslatePdfService _translatePdfService;

        public PdfController(
            IEmailToPdfConvertService emailToPdfConvertService,
            IWebPageToPdfConvertService webPageToPdfConvertService,
            ITranslatePdfService translatePdfService
        )
        {
            _emailToPdfConvertService = emailToPdfConvertService;
            _webPageToPdfConvertService = webPageToPdfConvertService;
            _translatePdfService = translatePdfService;
        }
        
        [Authorize]
        [HttpPost("from-email")]
        public async Task<string> EmailToPdfFromSource([FromBody, Required] EmailToPdfFromSourceRequestDto request)
        {
            return await _emailToPdfConvertService.Convert(request.SourceFileUrl, request.OutputFileName);
        }

        [Authorize]
        [HttpPost("from-web-page")]
        public async Task<string> WebPageToPdfFromSource([FromBody, Required] WebPageToPdfFromSourceRequestDto request)
        {
            return await _webPageToPdfConvertService.Convert(request.WebPageUrl);
        }

        [Authorize]
        [HttpPost("translate")]
        public async Task<string> Translate([FromBody, Required] TranslateRequestDto request)
        {
            return await _translatePdfService.Translate(request.SourceFileUrl, request.OutputFileName, request.LangFrom, request.LangTo);
        }
    }
}
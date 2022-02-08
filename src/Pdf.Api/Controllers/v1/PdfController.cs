using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pdf.Api.Dto.v1;
using Pdf.Api.External;
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

        public PdfController(IEmailToPdfConvertService emailToPdfConvertService)
        {
            _emailToPdfConvertService = emailToPdfConvertService;
        }
        
        [Authorize]
        [HttpPost("from-email")]
        public async Task<string> EmailToPdfFromSource([FromBody, Required] EmailToPdfFromSourceRequestDto request)
        {
            return await _emailToPdfConvertService.Convert(request.SourceFileUrl, request.OutputFileName);
        }
    }
}
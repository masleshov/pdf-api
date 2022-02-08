using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pdf.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion(Infrastructure.Constant.ApiVersion.V1)]
    [Route("/api/v{version:apiVersion}/diagnostic")]
    public sealed class DiagnosticController : ControllerBase 
    {
        [AllowAnonymous]
        [HttpGet("time")]
        public string GetServerTime() 
        {
            return "server time: " + DateTime.Now.TimeOfDay.ToString();
        }
    }
}
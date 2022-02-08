using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pdf.Api.Dto.v1;
using Pdf.Api.Infrastructure;
using Pdf.Api.Service;

namespace Pdf.Api.Controllers.v1
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion(Infrastructure.Constant.ApiVersion.V1)]
    [Route("/api/v{version:apiVersion}/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthorizationInfoService _authorizationInfoService;
        private readonly IEmailConfirmationService _emailConfirmationService;
        private readonly ILoginService _loginService;

        public AuthController(
            IAuthorizationInfoService authorizationInfoService,
            IEmailConfirmationService emailConfirmationService,
            ILoginService loginService
        )
        {
            _authorizationInfoService = authorizationInfoService;
            _emailConfirmationService = emailConfirmationService;
            _loginService = loginService;
        }

        [AllowAnonymous]
        [HttpPost("signup/send")]
        public async Task<SendEmailConfirmationResponseDto> SendEmailConfirmation([FromBody, Required] SendEmailConfirmationRequestDto request)
        {
            return await _emailConfirmationService.SendEmailConfirmationAsync(request.Email, request.Password);
        }

        [AllowAnonymous]
        [HttpPost("signup/confirm")]
        public async Task ConfirmEmail([FromBody, Required] ConfirmEmailRequestDto request) 
        {
            await _emailConfirmationService.ConfirmEmailAsync(request.ConfirmationToken, request.Code);
        }

        [AllowAnonymous]
        [HttpGet("token")]
        public async Task<GetTokenResponseDto> GetToken([FromQuery, Required] string email, string password)
        {
            return await _loginService.GetToken(email, password);
        }

        [Authorize]
        [HttpGet("token/refresh")]
        public async Task<RefreshTokenResponseDto> RefreshToken([FromQuery, Required] Guid refreshToken)
        {
            var customerId = AuthHelper.GetCustomerId(HttpContext);
            var authorizationInfo = await _authorizationInfoService.RefreshAuthorizationInfo(customerId, refreshToken);
            return new RefreshTokenResponseDto(authorizationInfo.AccessToken, authorizationInfo.RefreshToken, authorizationInfo.Expired);
        }
    }
}
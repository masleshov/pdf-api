using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pdf.Api.Repository;
using Pdf.Api.Service;

namespace Pdf.Api.Infrastructure
{
    internal sealed class JwtAuthenticationOptions : AuthenticationSchemeOptions
    {
    }

    internal sealed class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {
        private readonly CustomerRepository _customerRepository;
        private readonly IAuthorizationInfoService _authorizationInfoService;

        public JwtAuthenticationHandler(
            UnitOfWork uow,
            IAuthorizationInfoService authorizationInfoService,
            IOptionsMonitor<JwtAuthenticationOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _customerRepository = uow.GetRepository<CustomerRepository>();
            _authorizationInfoService = authorizationInfoService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Unauthorized");

            var header = Request.Headers["Authorization"].FirstOrDefault();
            if(string.IsNullOrEmpty(header)) return AuthenticateResult.NoResult();

            if (!header.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
 
            string token = header.Substring("bearer".Length).Trim();
            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var customerId = _authorizationInfoService.GetCustomerId(token);
            if(!customerId.HasValue) return AuthenticateResult.Fail("Unauthorized");

            var customer = await _customerRepository.GetCustomer(customerId.Value);
            if (customer == null) return AuthenticateResult.Fail("Unauthorized");

            // Last parameter is HUGE(!!!) important 
            // https://stackoverflow.com/questions/20254796/why-is-my-claimsidentity-isauthenticated-always-false-for-web-api-authorize-fil
            var identity = new ClaimsIdentity(new [] 
            {
                new Claim(ClaimTypes.NameIdentifier, customerId.ToString())
            }, Scheme.Name);
            var principal = new GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
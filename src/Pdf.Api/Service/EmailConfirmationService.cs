using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pdf.Api.Configuration;
using Pdf.Api.Domain;
using Pdf.Api.Dto.v1;
using Pdf.Api.Exception;
using Pdf.Api.Infrastructure;
using Pdf.Api.Repository;

namespace Pdf.Api.Service
{
    public interface IEmailConfirmationService
    {
        Task<SendEmailConfirmationResponseDto> SendEmailConfirmationAsync(string email, string password);
        Task ConfirmEmailAsync(Guid confirmationToken, string code);
    }

    internal sealed class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly IAuthorizationInfoService _authorizationInfoService;
        private readonly CustomerRepository _customerRepository;
        private readonly EmailConfirmationRepository _emailConfirmationRepository;
        private readonly EmailConfirmationConfiguration _emailConfirmationConfiguration;
        private readonly SmtpConfiguration _smtpConfiguration;

        public EmailConfirmationService(
            UnitOfWork uow,
            IAuthorizationInfoService authorizationInfoService,
            IOptions<EmailConfirmationConfiguration> emailConfirmationConfiguration,
            IOptions<SmtpConfiguration> smtpConfiguration
        )
        {
            _authorizationInfoService = authorizationInfoService;
            _customerRepository = uow.GetRepository<CustomerRepository>();
            _emailConfirmationRepository = uow.GetRepository<EmailConfirmationRepository>();
            _emailConfirmationConfiguration = emailConfirmationConfiguration.Value;
            _smtpConfiguration = smtpConfiguration.Value;
        }

        public async Task<SendEmailConfirmationResponseDto> SendEmailConfirmationAsync(string email, string password) 
        {
            var (salt, hash) = PasswordEncoder.Encode(password);
            var customerId = await _customerRepository.AddCustomerIfNotExists(new Customer(email, hash, salt));
            if (!customerId.HasValue)
            {
                throw new CustomerWithEmailAlreadyExistsException(email);
            }

            var confirmation = CreateNewEmailConfirmation(customerId.Value, email);
            var confirmationId = await _emailConfirmationRepository.AddConfirmationIfNotExistsAsync(confirmation);

            int ttl;
            if (!confirmationId.HasValue)
            {
                confirmation = await _emailConfirmationRepository.GetLastConfirmation(email);
                ttl = Convert.ToInt32((confirmation.Expired - DateTime.UtcNow).TotalMilliseconds);
                throw new EmailIsAlreadyConfirmingException(ttl);
            }

            SendEmail(email, confirmation.Code);

            ttl = Convert.ToInt32((confirmation.Expired - DateTime.UtcNow).TotalMilliseconds);
            return new SendEmailConfirmationResponseDto(confirmationId.Value, ttl);
        }

        public async Task ConfirmEmailAsync(Guid confirmationToken, string code)
        {
            var confirmation = await _emailConfirmationRepository.GetConfirmation(confirmationToken);
            if (confirmation == null || confirmation.Status != EmailConfirmationStatus.Pending)
            {
                throw new EmailConfirmationDoesNotExistsException(confirmationToken);
            }

            if (confirmation.Expired <= DateTime.UtcNow
                || confirmation.Attempts == _emailConfirmationConfiguration.UnsuccessfulAttemptsLimit)
            {
                await _emailConfirmationRepository.SetStatus(confirmationToken, EmailConfirmationStatus.Failed);
                throw new EmailConfirmationExpiredException(confirmationToken);
            }

            if (!confirmation.Code.Equals(code))
            {
                var attempts = confirmation.Attempts + 1;
                await _emailConfirmationRepository.SetAttempts(confirmationToken, attempts);
                throw new EmailConfirmationCodeWrongException(_emailConfirmationConfiguration.UnsuccessfulAttemptsLimit - attempts);
            }

            await _emailConfirmationRepository.SetStatus(confirmationToken, EmailConfirmationStatus.Success);
            await _customerRepository.SetStatus(confirmation.CustomerId, CustomerStatus.Active);
        }

        private EmailConfirmation CreateNewEmailConfirmation(int customerId, string email)
        {
            var min = (int)Math.Pow(10, _emailConfirmationConfiguration.CodeLength - 1);
            var max = (int)Math.Pow(10, _emailConfirmationConfiguration.CodeLength) - 1;

            var random = new Random((int)DateTime.UtcNow.Ticks & 0x0000FFFF)
                .Next(min, max);

            return new EmailConfirmation(customerId, email, random.ToString(), DateTime.UtcNow.AddSeconds(_emailConfirmationConfiguration.RequestTtl));
        }

        private void SendEmail(string email, string code) 
        {
            var smtpClient = new SmtpClient(_smtpConfiguration.Host)
            {
                Port = 587,
                Credentials = new NetworkCredential(_smtpConfiguration.Login, _smtpConfiguration.Password),
                EnableSsl = true,
            };
                
            smtpClient.Send(_smtpConfiguration.Login, email, "Email confirmation", $"Your confirmation code: {code}");
        }
    }
}
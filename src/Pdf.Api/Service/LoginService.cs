using System;
using System.Threading.Tasks;
using Pdf.Api.Domain;
using Pdf.Api.Dto.v1;
using Pdf.Api.Exception;
using Pdf.Api.Infrastructure;
using Pdf.Api.Repository;

namespace Pdf.Api.Service
{
    public interface ILoginService
    {
        Task<GetTokenResponseDto> GetToken(string email, string password);
    }

    internal sealed class LoginService : ILoginService
    {
        private readonly CustomerRepository customerRepository;
        private readonly IAuthorizationInfoService _authorizationInfoService;

        public LoginService(UnitOfWork uow, IAuthorizationInfoService authorizationInfoService)
        {
            customerRepository = uow.GetRepository<CustomerRepository>();
            _authorizationInfoService = authorizationInfoService;
        }

        public async Task<GetTokenResponseDto> GetToken(string email, string password) 
        {
            var customers = await customerRepository.GetCustomer(email);
            if (customers == null || customers.Length == 0) throw new CustomerDoesNotExistException(email);
            if (customers.Length != 1) throw new InvalidOperationException($"Received more than one customer for email {email}");

            var customer = customers[0];
            if (customer.Status != CustomerStatus.Active) throw new CustomerIsNotActiveException(email);

            if (!PasswordEncoder.CheckPassword(password, customer.PasswordSalt, customer.PasswordHash))
            {
                throw new CustomerDoesNotExistException(email);
            }

            var authorizationInfo = await _authorizationInfoService.GetAuthorizationInfo(customer.CustomerId);
            return new GetTokenResponseDto(authorizationInfo.AccessToken, authorizationInfo.RefreshToken, authorizationInfo.Expired);
        }
    }
}
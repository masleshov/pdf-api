using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Pdf.Api.Domain;
using Pdf.Api.Exception;
using Pdf.Api.Infrastructure;
using Pdf.Api.Infrastructure.Constant;
using Pdf.Api.Repository;

namespace Pdf.Api.Service
{
    public interface IAuthorizationInfoService
    {
        Task<AuthorizationInfo> GetAuthorizationInfo(int customerId);
        Task<AuthorizationInfo> RefreshAuthorizationInfo(int customerId, Guid refreshToken);
        int? GetCustomerId(string token);
    }

    internal sealed class AuthorizationInfoService : IAuthorizationInfoService
    {
        private const int TokenExpires = 15 * 60 * 1000;

        private readonly AuthorizationInfoRepository _repository;
        private readonly JwtSecurityTokenHandler _handler;

        public AuthorizationInfoService(UnitOfWork uow) 
        {
            _repository = uow.GetRepository<AuthorizationInfoRepository>();
            _handler = new JwtSecurityTokenHandler();
        }

        public async Task<AuthorizationInfo> GetAuthorizationInfo(int customerId)
        {
            var authorizationInfo = await _repository.GetAuthorizationInfo(customerId);
            if (authorizationInfo != null && authorizationInfo.Expired > DateTime.UtcNow)
            {
                return authorizationInfo;
            }

            var expired = DateTime.UtcNow.AddMilliseconds(TokenExpires);
            var accessToken = GenerateToken(customerId, expired);

            var insert = authorizationInfo == null;

            authorizationInfo = new AuthorizationInfo(
                customerId: customerId,
                accessToken: _handler.WriteToken(accessToken),
                refreshToken: Guid.NewGuid().ToString(),
                expired: expired
            );

            if (insert) await _repository.AddAuthorizationInfo(authorizationInfo);
            else await _repository.UpdateAuthorizationInfo(authorizationInfo);
            
            return authorizationInfo;
        }

        public async Task<AuthorizationInfo> RefreshAuthorizationInfo(int customerId, Guid refreshToken)
        {
            var stored = await _repository.GetAuthorizationInfo(customerId);
            if(stored == null) throw new InvalidRefreshTokenException("Wrong client id");

            if(!Guid.TryParse(stored.RefreshToken, out var storedRefreshToken) 
                || storedRefreshToken != refreshToken) 
                throw new InvalidRefreshTokenException("Wrong refresh token");

            if(stored.Expired < DateTime.UtcNow) throw new InvalidRefreshTokenException("Refresh token is expired");

            var expired = DateTime.UtcNow.AddMilliseconds(TokenExpires);
            var accessToken = GenerateToken(customerId, expired);
            var authorizationInfo = new AuthorizationInfo(
                customerId: customerId,
                accessToken: _handler.WriteToken(accessToken),
                refreshToken: Guid.NewGuid().ToString(),
                expired: expired
            );

            await _repository.UpdateAuthorizationInfo(authorizationInfo);

            return authorizationInfo;
        }

        public int? GetCustomerId(string token)
        {
            try
            {
                var claims = _handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = JwtConstant.Issuer,
                    ValidAudience = JwtConstant.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtConstant.Secret))
                }, out SecurityToken validatedToken);

                var nameIdentifier = claims.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
                if(nameIdentifier == null) throw new NullReferenceException("Wrong JWT. There is no one name identifier");

                return Convert.ToInt32(nameIdentifier.Value);
            }
            catch(System.Exception ex)
            {
                // TODO: log exception
                return null;
            }
        }

        private JwtSecurityToken GenerateToken(int clientId, DateTime expires)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, clientId.ToString())
                }),
                Expires = expires,
                Issuer = JwtConstant.Issuer,
                Audience = JwtConstant.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtConstant.Secret)), SecurityAlgorithms.HmacSha256Signature)
            };

            return _handler.CreateJwtSecurityToken(tokenDescriptor);
        }
    }
}
using System;

namespace Pdf.Api.Domain
{
    public sealed record AuthorizationInfo
    {
        public int CustomerId { get; init; }
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
        public DateTime Expired { get; init; }
        public DateTime CreateTimestamp { get; init; }
        public DateTime UpdateTimestamp { get; init; }

        /// <summary>
        /// Parameterless constructor to use class in dapper mapping
        /// </summary>
        public AuthorizationInfo()
        {

        }

        public AuthorizationInfo(int customerId, string accessToken, string refreshToken, DateTime expired)
        {
            CustomerId = customerId;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Expired = expired;
        }
    }
}
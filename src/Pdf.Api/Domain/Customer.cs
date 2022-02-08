using System;

namespace Pdf.Api.Domain
{
    internal sealed record Customer
    {
        public int CustomerId { get; init; }
        public string Email { get; init; }
        public string PasswordHash { get; init; }
        public string PasswordSalt { get; init; }
        public CustomerStatus Status { get; init; }
        public DateTime CreateTimestamp { get; init; }
        public DateTime UpdateTimestamp { get; init; }

        /// <summary>
        /// Parameterless constructor to use class in dapper mapping
        /// </summary>
        public Customer() 
        {

        }

        public Customer(string email, string passwordHash, string passwordSalt)
        {
            Email = email;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Status = CustomerStatus.NotActive;
            CreateTimestamp = DateTime.UtcNow;
            UpdateTimestamp = DateTime.UtcNow;
        }
    }
}
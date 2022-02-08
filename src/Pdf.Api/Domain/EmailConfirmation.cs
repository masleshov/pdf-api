using System;

namespace Pdf.Api.Domain
{
    internal sealed class EmailConfirmation
    {
        public Guid ConfirmationId { get; init; }
        public int CustomerId { get; init; }
        public string Email { get; init; }
        public string Code { get; init; }
        public DateTime Expired { get; init; }
        public int Attempts { get; init; }
        public EmailConfirmationStatus Status { get; init; }
        public DateTime CreateTimestamp { get; init; }
        public DateTime UpdateTimestamp { get; init; }

        /// <summary>
        /// Parameterless constructor to use class in dapper mapping
        /// </summary>
        public EmailConfirmation()
        {

        }

        public EmailConfirmation(int customerId, string email, string code, DateTime expired)
        {
            CustomerId = customerId;
            Email = email;
            Code = code;
            Expired = expired;
            Attempts = 0;
            Status = EmailConfirmationStatus.Pending;
            CreateTimestamp = DateTime.UtcNow;
            UpdateTimestamp = DateTime.UtcNow;
        }
    }
}
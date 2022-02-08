using System;

namespace Pdf.Api.Exception
{
    internal sealed class EmailConfirmationExpiredException : System.Exception
    {
        public EmailConfirmationExpiredException(Guid confirmationToken) 
            : base($"Email confirmation with token {confirmationToken} expired")
        {
        }
    }
}
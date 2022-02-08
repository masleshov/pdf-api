using System;

namespace Pdf.Api.Exception
{
    internal sealed class EmailConfirmationDoesNotExistsException : System.Exception
    {
        public EmailConfirmationDoesNotExistsException(Guid confirmationId) 
            : base($"No one pending email confirmation has been found by token {confirmationId}")
        {
        }
    }
}
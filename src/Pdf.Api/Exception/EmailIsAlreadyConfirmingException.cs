using Pdf.Api.Domain;

namespace Pdf.Api.Exception
{
    internal sealed class EmailIsAlreadyConfirmingException : System.Exception
    {
        public EmailIsAlreadyConfirmingException(int ttl) 
            : base($"Phone confirmation in status {EmailConfirmationStatus.Pending} already exists. TTL: {ttl}")
        {
        }
    }
}
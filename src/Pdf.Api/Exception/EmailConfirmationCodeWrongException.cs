namespace Pdf.Api.Exception
{
    internal sealed class EmailConfirmationCodeWrongException : System.Exception
    {
        public EmailConfirmationCodeWrongException(int remainingAttempts) 
            : base($"Wrong confirmation code. Attempts remain: {remainingAttempts}")
        {
        }
    }
}
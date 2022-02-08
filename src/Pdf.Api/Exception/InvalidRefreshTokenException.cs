namespace Pdf.Api.Exception
{
    public sealed class InvalidRefreshTokenException : System.Exception
    {
        public InvalidRefreshTokenException(string message) : base(message)
        {
        }
    }
}
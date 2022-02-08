namespace Pdf.Api.Exception
{
    public sealed class PdfApiException : System.Exception
    {
        public PdfApiException(string message) : base(message)
        {
        }
    }
}
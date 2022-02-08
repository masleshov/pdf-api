namespace Pdf.Api.Configuration
{
    public record SmtpConfiguration
    {
        public string Host { get; init; }
        public int Port { get; init; }
        public string Login { get; init; }
        public string Password { get; init; }
    }
}
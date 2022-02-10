namespace Pdf.Api.Configuration
{
    public record RequestThrottlingConfiguration
    {
        public bool Enabled { get; init; }
        public int Delay { get; init; }
    }
}
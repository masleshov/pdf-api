namespace Pdf.Api.Configuration
{
    public record EmailConfirmationConfiguration
    {
        /// <summary>
        /// Length of confirmation code (in digits)
        /// </summary>
        public int CodeLength { get; init; }

        /// <summary>
        /// Active request Time To Live (seconds)
        /// </summary>
        public int RequestTtl { get; init; }
        
        /// <summary>
        /// Maximum number of unsuccessful attempts to enter code
        /// </summary>
        public int UnsuccessfulAttemptsLimit { get; init; }
    }
}
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Pdf.Api.Exception.DetailConverter
{
    internal sealed class EmailConfirmationExpiredExceptionDetailConverter : IDetailConverter<EmailConfirmationExpiredException>
    {
        public ProblemDetails Convert(EmailConfirmationExpiredException exception)
        {
            return new ProblemDetails
            {
                Status = (int)HttpStatusCode.Unauthorized,
                Title = "/unauthorized",
                Detail = exception.Message
            };
        }
    }
}
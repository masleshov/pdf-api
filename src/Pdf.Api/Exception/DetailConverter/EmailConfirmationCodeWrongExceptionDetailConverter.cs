using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Pdf.Api.Exception.DetailConverter
{
    internal sealed class EmailConfirmationCodeWrongExceptionDetailConverter : IDetailConverter<EmailConfirmationCodeWrongException>
    {
        public ProblemDetails Convert(EmailConfirmationCodeWrongException exception)
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
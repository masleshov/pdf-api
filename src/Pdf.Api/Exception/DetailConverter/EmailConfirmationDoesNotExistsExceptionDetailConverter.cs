using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Pdf.Api.Exception.DetailConverter
{
    internal sealed class EmailConfirmationDoesNotExistsExceptionDetailConverter : IDetailConverter<EmailConfirmationDoesNotExistsException>
    {
        public ProblemDetails Convert(EmailConfirmationDoesNotExistsException exception)
        {
            return new ProblemDetails
            {
                Status = (int)HttpStatusCode.NotFound,
                Title = "/not-found",
                Detail = exception.Message
            };
        }
    }
}
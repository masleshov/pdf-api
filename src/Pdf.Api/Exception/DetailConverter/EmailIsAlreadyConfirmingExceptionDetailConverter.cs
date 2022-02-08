using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Pdf.Api.Exception.DetailConverter
{
    internal sealed class PhoneIsAlreadyConfirmingExceptionDetailConverter : IDetailConverter<EmailIsAlreadyConfirmingException>
    {
        public ProblemDetails Convert(EmailIsAlreadyConfirmingException exception)
        {
            return new ProblemDetails
            {
                Status = (int)HttpStatusCode.TooManyRequests,
                Title = "/too-many-requests",
                Detail = exception.Message
            };
        }
    }
}
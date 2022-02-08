using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Pdf.Api.Exception.DetailConverter
{
    internal sealed class CustomerWithEmailAlreadyExistsExceptionDetailConverter : IDetailConverter<CustomerWithEmailAlreadyExistsException>
    {
        public ProblemDetails Convert(CustomerWithEmailAlreadyExistsException exception)
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
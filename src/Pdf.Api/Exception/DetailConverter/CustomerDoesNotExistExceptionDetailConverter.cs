using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Pdf.Api.Exception.DetailConverter
{
    internal sealed class CustomerDoesNotExistExceptionDetailConverter : IDetailConverter<CustomerDoesNotExistException>
    {
        public ProblemDetails Convert(CustomerDoesNotExistException exception)
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
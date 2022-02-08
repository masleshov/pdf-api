using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Pdf.Api.Exception.DetailConverter
{
    internal sealed class CustomerIsNotActiveExceptionDetailConverter : IDetailConverter<CustomerIsNotActiveException>
    {
        public ProblemDetails Convert(CustomerIsNotActiveException exception)
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
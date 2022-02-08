using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Pdf.Api.Exception.DetailConverter
{
    internal sealed class PdfApiExceptionDetailConverter : IDetailConverter<PdfApiException>
    {
        public ProblemDetails Convert(PdfApiException exception)
        {
            return new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadGateway,
                Title = "/bad-gateway",
                Detail = exception.Message
            };
        }
    }
}
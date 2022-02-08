using Microsoft.AspNetCore.Mvc;

namespace Pdf.Api.Exception.DetailConverter
{
    internal interface IDetailConverter<TException> where TException : System.Exception
    {
        ProblemDetails Convert(TException exception);
    }
}
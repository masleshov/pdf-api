using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Pdf.Api.Infrastructure
{
    internal static class AuthHelper
    {
        public static int GetCustomerId(HttpContext context)
        {
            return Convert.ToInt32(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
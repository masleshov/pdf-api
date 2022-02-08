using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Pdf.Api.Exception.DetailConverter;

namespace Pdf.Api.Infrastructure
{
    internal static class ResponseExceptionHandler
    {
        private static readonly Dictionary<Type, Func<System.Exception, ProblemDetails>> _detailConverters;

        static ResponseExceptionHandler()
        {
            _detailConverters = Assembly.GetExecutingAssembly().DefinedTypes
                .Select(type => new
                {
                    Type = type,
                    ExceptionType = type.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDetailConverter<>))
                        .Select(i => i.GenericTypeArguments[0])
                        .FirstOrDefault()
                })
                .Where(couple => couple.ExceptionType != null)
                .ToDictionary(key => key.ExceptionType, value => GetCompiledDetailConverterFunc(value.Type));
        }

        public static void UseResponseExceptionHandler(this IApplicationBuilder app, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.Use(WriteDevelopmentResponse);
            }
            else
            {
                app.Use(WriteProductionResponse);
            }
        }

        private static Task WriteDevelopmentResponse(HttpContext httpContext, Func<Task> next)
            => WriteResponse(httpContext, true);

        private static Task WriteProductionResponse(HttpContext httpContext, Func<Task> next)
            => WriteResponse(httpContext, false);

        private static async Task WriteResponse(HttpContext httpContext, bool includeDetails)
        {
            var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = exceptionDetails?.Error;
            if (ex == null)
                return;

            httpContext.Response.ContentType = "application/problem+json";

            var detailConverter = _detailConverters.GetValueOrDefault(ex.GetType());
            var problem = detailConverter != null
                ? detailConverter(ex)
                : new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = includeDetails ? "An error occurred: " + ex.Message : "An error occurred",
                    Detail = includeDetails ? ex.ToString() : null
                };

            problem.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            if (problem.Status.HasValue)
                httpContext.Response.StatusCode = problem.Status.Value;

            var stream = httpContext.Response.Body;
            await JsonSerializer.SerializeAsync(stream, problem);
        }

        private static Func<System.Exception, ProblemDetails> GetCompiledDetailConverterFunc(Type type)
        {
            var methodInfo = type.GetMethod("Convert");
            var parameterExpression = Expression.Parameter(typeof(System.Exception));
            var parameterType = methodInfo.GetParameters()[0].ParameterType;
            var methodParameter = Expression.Convert(parameterExpression, parameterType);
            var callExpression = Expression.Call(Expression.MemberInit(Expression.New(type)), methodInfo, methodParameter);
            return Expression.Lambda<Func<System.Exception, ProblemDetails>>(callExpression, parameterExpression).Compile();
        }
    }
}
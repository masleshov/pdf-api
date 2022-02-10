using System;
using System.Collections.Concurrent;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pdf.Api.Configuration;
using Pdf.Api.Exception;

namespace Pdf.Api.Infrastructure
{
    internal sealed class RequestThrottlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestThrottlingConfiguration _configuration;
        private readonly ConcurrentDictionary<RequestKey, DateTime> _requests;
        
        public RequestThrottlingMiddleware(RequestDelegate next, IOptions<RequestThrottlingConfiguration> configuration)
        {
            _next = next;
            _configuration = configuration.Value;
            _requests = new ConcurrentDictionary<RequestKey, DateTime>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!_configuration.Enabled) return;

            var endpoint = context.GetEndpoint();
            var ip = context.Connection.RemoteIpAddress?.ToString();

            var key = new RequestKey(endpoint, ip);
            if (_requests.TryGetValue(key, out var allowedExecutionTime))
            {
                if (allowedExecutionTime > DateTime.UtcNow)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    return;
                }

                _requests.TryRemove(key, out var _);
            }

            _requests.TryAdd(key, DateTime.UtcNow.AddMilliseconds(_configuration.Delay));
            await _next(context);
        }

        private struct RequestKey
        {
            public readonly Endpoint Endpoint;
            public readonly string IncomingIp;

            public RequestKey(Endpoint endpoint, string incomingIp)
            {
                Endpoint = endpoint;
                IncomingIp = incomingIp;
            }
        }
    }
}
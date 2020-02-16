using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VS_LOAN.Core.Utility;

namespace VietBankApi.Infrastructures
{
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            //var path = httpContext.Request.Path;
            //if(!path.HasValue)
            //{
            //    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    return;
            //}
            //var key = httpContext.Request.Headers["X-VietbankFC-Signature"].FirstOrDefault();
            //if(string.IsNullOrWhiteSpace(key))
            //{
            //    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    return;
            //}
            //var apiKey = Utility.HmacSha256("VietbankFc", "everbodyknowthatthecaptainlied");
            //if(key != apiKey)
            //{
            //    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    return;
            //}
            await _next(httpContext);
        }
    }
}

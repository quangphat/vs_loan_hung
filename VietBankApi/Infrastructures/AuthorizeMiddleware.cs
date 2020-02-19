using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VietBankApi.Business.Interfaces;

namespace VietBankApi.Infrastructures
{
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext, IAuthorizeBusiness authorizeBusiness, CurrentProcess currentProcess)
        {
            //var path = httpContext.Request.Path;
            //if (!path.HasValue)
            //{
            //    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    return;
            //}
            //var key = httpContext.Request.Headers["X-VietbankFC-Signature"].FirstOrDefault();
            //if (string.IsNullOrWhiteSpace(key))
            //{
            //    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    return;
            //}
            //var apiKey = Utils.HmacSha256("VietbankFc", "everbodyknowthatthecaptainlied");
            //if (key != apiKey)
            //{
            //    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    return;
            //}
            //var token = await authorizeBusiness.GetToken();
            //currentProcess.Token = token;
            await _next(httpContext);
        }
    }
}

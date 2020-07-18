using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace KingOffice.Infrastructures
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class MyAuthorize : ActionFilterAttribute, IActionFilter
    {
        public string Roles { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        { 

            var result = AuthorizeCore(context.HttpContext);
            if (result)
                return;
            context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary { { "Controller", "Account" }, { "Action", "Login" } });
        }

        protected bool AuthorizeCore(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }


            var user = httpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }
            List<Claim> list = user.Claims?.ToList();
            
            if (list == null && list.Count == 0)
                return false;
            string scopeStr = list.FirstOrDefault((Claim a) => a.Type == "Scopes")?.Value;
            
            if (string.IsNullOrWhiteSpace(scopeStr))
            {
                if (string.IsNullOrWhiteSpace(this.Roles))
                    return true;
                return false;
            }
            var userScopes = scopeStr.Split(',').ToArray();
            if (string.IsNullOrWhiteSpace(this.Roles))
                return true;
            var roles = this.Roles.Split(',').ToArray();
            var result = false;
            foreach(var r in userScopes)
            {
                var exist = roles.FirstOrDefault(p => p == r);
                if (exist == null)
                    continue;
                else
                    result = true;
            }
            return result;
        }
    }
}

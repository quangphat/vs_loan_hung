﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VietStar.Entities.Infrastructures;
using VietStar.Utility;

namespace KingOffice.Infrastructures
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class MyAuthorize : ActionFilterAttribute, IActionFilter
    {
        public string Permissions { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
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
            if (list == null || !list.Any())
                return false;
            string scopeStr = list.FirstOrDefault((Claim a) => a.Type == "Scopes")?.Value;

            if (string.IsNullOrWhiteSpace(scopeStr))
            {
                if (string.IsNullOrWhiteSpace(Permissions))
                    return true;
                return false;
            }
            if (string.IsNullOrWhiteSpace(Permissions))
                return true;
            var userScopes = scopeStr.Split(',').ToList();
            var allowPermission = Permissions.Split(',').ToList();
            var isInRole = userScopes.Any(x => allowPermission.Contains(x));
            if (!isInRole)
                return false;

            return true;
        }
        
    }
}
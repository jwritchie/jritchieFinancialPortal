﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace jritchieFinancialPortal.Models.Helpers
{
    public class AuthorizeHouseholdRequired : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if(!isAuthorized)
            {
                return false;
            }

            return httpContext.User.Identity.IsInHousehold();
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if(!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);      // Redirect user to login.
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult
                    (new RouteValueDictionary
                    (new { controller = "Households", action = "Create" } ));
            }
        }
    }
}
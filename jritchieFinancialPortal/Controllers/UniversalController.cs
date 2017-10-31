using jritchieFinancialPortal.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jritchieFinancialPortal.Controllers
{
    [Authorize]
    public class UniversalController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                ViewBag.UserTimeZone = user.TimeZone;

            }

            base.OnActionExecuting(filterContext);
        }

    }
}
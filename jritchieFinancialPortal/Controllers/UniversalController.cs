using jritchieFinancialPortal.Models;
using jritchieFinancialPortal.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jritchieFinancialPortal.Controllers
{
    [RequireHttps]
    [Authorize]
    public class UniversalController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                if (user != null)
                {
                    ViewBag.CurrentUserId = user.Id;
                    ViewBag.UserTimeZone = user.TimeZone;
                    ViewBag.CurrentUserName = user.Fullname;

                    ViewBag.CurrentUserHouseholdId = user.HouseholdId;
                    if (user.HouseholdId != null)
                    {
                        ViewBag.CurrentUserHouseholdName = user.Household.Name;
                    }

                    ViewBag.CurrentUserBanksCount = db.Banks.Where(b => b.HouseholdId == user.HouseholdId).Count();

                    List<BankAccount> accounts = new List<BankAccount>();
                    accounts = db.BankAccounts.Where(a => a.HouseholdId == user.HouseholdId).Where(a => a.Balance < 0).ToList();
                    if (accounts.Count > 0)
                    {
                        if (accounts.Count == 1)
                        {
                            ViewBag.OverdrawnWarning = "Warning: An account is overdrawn.";
                        }
                        else
                        {
                            ViewBag.OverdrawnWarning = "Warning: Multiple accounts are overdrawn.";
                        }
                    }
                    else
                    {
                        ViewBag.OverdrawnWarning = "";
                    }

                }
            }

            base.OnActionExecuting(filterContext);
        }

    }
}
using jritchieFinancialPortal.Models.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace jritchieFinancialPortal.Controllers
{
    [Authorize]
    public class HomeController : UniversalController
    {
        //[AuthorizeHouseholdRequired]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        // underscore denotes a partial view.
        public PartialViewResult _Contact(string id)
        {
            ViewBag.Message = id;

            return PartialView();
        }
    }
}
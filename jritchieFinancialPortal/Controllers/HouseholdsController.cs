using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jritchieFinancialPortal.Models;
using jritchieFinancialPortal.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using jritchieFinancialPortal.Models.Helpers;

namespace jritchieFinancialPortal.Controllers
{
    public class HouseholdsController : UniversalController
    {
        [AuthorizeHouseholdRequired]
        // GET: Households ... Is in a household?
        public ActionResult IsInHousehold()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (User.IsInRole("Admin"))
            {
                ViewBag.UserTimeZone = user.TimeZone;
                return RedirectToAction("Index");
            }
            else
            {
                if (user.HouseholdId != null)
                {
                    ViewBag.UserTimeZone = user.TimeZone;
                    return RedirectToAction("Details", "Households", new { id = user.HouseholdId } );
                }
                else
                {
                    ViewBag.UserTimeZone = user.TimeZone;
                    return RedirectToAction("Create");
                }
            }
        }

        [AuthorizeHouseholdRequired]
        // GET: Households
        public ActionResult Index()
        {

            if (User.IsInRole("Admin"))
            {
                List<HouseholdUserViewModel> householdUserVMList = new List<HouseholdUserViewModel>();
                foreach (var household in db.Households.ToList())
                {
                    HouseholdUserViewModel householdUserVM = new HouseholdUserViewModel();
                    householdUserVM.Household = household;

                    householdUserVM.SelectedUsers = household.Users.Where(u => u.HouseholdId == household.Id).ToList();    // users in household.

                    householdUserVM.SelectedUsersName = household.Users.OrderBy(u => u.LastName).Select(u => u.Fullname).ToArray();

                    householdUserVMList.Add(householdUserVM);
                }

                return View(householdUserVMList);
                //return View(db.Households.ToList());
            }
            else
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                return RedirectToAction("Details", "Households", new { id = user.HouseholdId } );
            }
        }

        [AuthorizeHouseholdRequired]
        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HouseholdUserViewModel householdUserVM = new HouseholdUserViewModel();
            householdUserVM.Household = db.Households.Find(id);

            householdUserVM.SelectedUsers = householdUserVM.Household.Users.ToList();

            householdUserVM.SelectedUsersName = householdUserVM.Household.Users.OrderBy(u => u.LastName).Select(u => u.Fullname).ToArray();

            //Household household = db.Households.Find(id);
            if (householdUserVM == null)
            {
                return HttpNotFound();
            }

            return View(householdUserVM);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Established")] Household household)
        {
            if (ModelState.IsValid)
            {
                household.Established = DateTimeOffset.UtcNow;

                db.Households.Add(household);
                db.SaveChanges();

                foreach(var category in db.CategoriesGeneric)
                {
                    Category seedNewHouse = new Category();
                    seedNewHouse.Name = category.Name;
                    seedNewHouse.Description = category.Description;
                    seedNewHouse.TransactionTypeId = category.TransactionTypeId;
                    seedNewHouse.HouseholdId = household.Id;

                    db.Categories.Add(seedNewHouse);
                }
                db.SaveChanges();

                var user = db.Users.Find(User.Identity.GetUserId());
                user.HouseholdId = household.Id;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                await HttpContext.RefreshAuthentication(user);  // join POST & leave POST
                return RedirectToAction("Details", "Households", new { id = user.HouseholdId } );
                //return RedirectToAction("Index");
            }

            return View(household);
        }

        [AuthorizeHouseholdRequired]
        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        [AuthorizeHouseholdRequired]
        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Established")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        //[AuthorizeHouseholdRequired]
        //// GET: Households/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Household household = db.Households.Find(id);
        //    if (household == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(household);
        //}

        //[AuthorizeHouseholdRequired]
        //// POST: Households/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Household household = db.Households.Find(id);
        //    db.Households.Remove(household);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}


        //public async Task<ActionResult> JoinHousehold(int householdId)
        //{
        //    // Implementation for joining a household.
        //    var user = db.Users.Find(User.Identity.GetUserId());
        //    user.HouseholdId = householdId;

        //    db.Entry(user).State = EntityState.Modified;
        //    db.SaveChanges();

        //    await ControllerContext.HttpContext.RefreshAuthentication(user);

        //    ViewBag.UserTimeZone = user.TimeZone;

        //    return RedirectToAction("Details", "Households", new { id = user.HouseholdId });
        //    //return View();
        //}

        // POST: LeaveHousehold
        public async Task<ActionResult> LeaveHousehold()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            user.HouseholdId = null;

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            // Implementation of leaving household (flush browser cookie).
            await ControllerContext.HttpContext.RefreshAuthentication(user);
            return RedirectToAction("Index","Home");
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

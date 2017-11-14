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
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using jritchieFinancialPortal.Models.Helpers;

namespace jritchieFinancialPortal.Controllers
{
    [RequireHttps]
    [AuthorizeHouseholdRequired]
    public class InvitationsController : UniversalController
    {
        // GET: Invitations
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var invitations = db.Invitations.Include(i => i.Household);
                return View(invitations.ToList());
            }
            else
            {
                var userHouseholdId = db.Users.Find(User.Identity.GetUserId()).HouseholdId;
                var invitations = db.Invitations.Where(i => i.HouseholdId == userHouseholdId);
                return View(invitations.ToList());
            }

        }

        // GET: Invitations/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Invitation invitation = db.Invitations.Find(id);
        //    if (invitation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(invitation);
        //}

        // GET: Invitations/Create
        //public ActionResult Create()
        //{
        //    ViewBag.HouseholdId = db.Users.Find(User.Identity.GetUserId()).HouseholdId;
        //    //ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Id");
        //    return View();
        //}

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,HouseholdId,Email,DateTimeIssued,Accept")] Invitation invitation)
        {
            bool inviteeExists = db.Users.Any(u => u.Email == invitation.Email);
            if (inviteeExists)
            {
                int? inviteeHasHousehold = db.Users.FirstOrDefault(u => u.Email == invitation.Email).HouseholdId;
                if (inviteeHasHousehold != null)
                {
                    return View("InviteeHasHousehold");
                }
            }

            if (ModelState.IsValid)
            {
                invitation.DateTimeIssued = DateTimeOffset.UtcNow;
                invitation.DateTimeExpires = DateTimeOffset.UtcNow.AddMinutes(10);
                invitation.PasswordGUID = Guid.NewGuid().ToString();

                db.Invitations.Add(invitation);
                db.SaveChanges();

                try
                {
                    var body = "<p>{0}</p><p>{1}</p>";
                    var from = "FP<jritchie.projects@gmail.com>";
                    var callbackUrl = Url.Action("RegisterInvitee", "Account", new { code = invitation.PasswordGUID, email = invitation.Email }, protocol: Request.Url.Scheme);


                    var email = new MailMessage(from, invitation.Email)
                    {
                        Subject = "FP Notification Email: Invitation issued",
                        Body = string.Format(body, "Message from FP:", "You have been invited to join a FP Household. " +
                                                   "Please click <a href=\"" + callbackUrl + "\">here</a> to join.  " + 
                                                   "NOTE: This invitation will expire at: '" + invitation.DateTimeExpires.ToString("MM/dd/yyyy | hh:mm:ss")  + "' UTC."),
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }

                return RedirectToAction("Index");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
            return View(invitation);
        }

        // GET: Invitations/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Invitation invitation = db.Invitations.Find(id);
        //    if (invitation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
        //    return View(invitation);
        //}

        // POST: Invitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,HouseholdId,Email,DateTimeIssued,Accept")] Invitation invitation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(invitation).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
        //    return View(invitation);
        //}

        // GET: Invitations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invitation invitation = db.Invitations.Find(id);
            db.Invitations.Remove(invitation);
            db.SaveChanges();
            return RedirectToAction("Index");
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

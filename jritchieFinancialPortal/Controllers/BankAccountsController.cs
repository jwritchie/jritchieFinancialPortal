﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jritchieFinancialPortal.Models;
using jritchieFinancialPortal.Models.CodeFirst;
using jritchieFinancialPortal.Models.Helpers;

namespace jritchieFinancialPortal.Controllers
{
    [RequireHttps]
    [AuthorizeHouseholdRequired]
    public class BankAccountsController : UniversalController
    {
        // GET: BankAccounts
        public ActionResult Index()
        {
            var currentHouseholdId = User.Identity.GetHouseholdId();
            var bankAccounts = db.BankAccounts.Where(b => b.HouseholdId == currentHouseholdId).OrderBy(b => b.Bank.Name).ThenBy(b => b.Name);
            //var bankAccounts = db.BankAccounts.Include(b => b.Bank).Include(b => b.Household);
            return View(bankAccounts.ToList());
        }

        // GET: BankAccounts/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BankAccount bankAccount = db.BankAccounts.Find(id);
        //    if (bankAccount == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(bankAccount);
        //}

        // GET: BankAccounts/Create
        public ActionResult Create()
        {
            var currentHouseholdId = User.Identity.GetHouseholdId();
            List<Bank> currentUserBank = new List<Bank>();
            currentUserBank = db.Banks.Where(b => b.HouseholdId == currentHouseholdId).ToList();

            if (currentUserBank.Count != 0)
            {
                //ViewBag.BankId = new SelectList(db.Banks, "Id", "Name");
                ViewBag.BankId = new SelectList(currentUserBank, "Id", "Name");
                ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
                return View();
            }
            else
            {
                return View("Error");
            }
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Balance,Name,Opened,Closed,HouseholdId,BankId")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                bankAccount.Balance = 0.0M;
                bankAccount.BalanceReconciled = 0.0M;
                bankAccount.HouseholdId = ViewBag.CurrentUserHouseholdId;
                bankAccount.Opened = DateTimeOffset.UtcNow;

                db.BankAccounts.Add(bankAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var currentHouseholdId = User.Identity.GetHouseholdId();
            List<Bank> currentUserBank = new List<Bank>();
            currentUserBank = db.Banks.Where(b => b.HouseholdId == currentHouseholdId).ToList();

            //ViewBag.BankId = new SelectList(db.Banks, "Id", "Name", bankAccount.BankId);
            ViewBag.BankId = new SelectList(currentUserBank, "Id", "Name", bankAccount.BankId);
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseholdId);
            return View(bankAccount);
        }

        // GET: BankAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }

            var currentHouseholdId = User.Identity.GetHouseholdId();
            List<Bank> currentUserBank = new List<Bank>();
            currentUserBank = db.Banks.AsNoTracking().Where(b => b.HouseholdId == currentHouseholdId).ToList();

            ViewBag.BankId = new SelectList(currentUserBank, "Id", "Name", bankAccount.BankId);
            //ViewBag.BankId = new SelectList(db.Banks, "Id", "Name", bankAccount.BankId);
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseholdId);
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Balance,Name,Opened,Closed,HouseholdId,BankId")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                var local = db.Set<BankAccount>().Local.FirstOrDefault(l => l.Id == bankAccount.Id);
                if (local != null)
                {
                    db.Entry(local).State = EntityState.Detached;
                }

                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var currentHouseholdId = User.Identity.GetHouseholdId();
            List<Bank> currentUserBanks = new List<Bank>();
            currentUserBanks = db.Banks.Where(b => b.HouseholdId == currentHouseholdId).ToList();

            //ViewBag.BankId = new SelectList(db.Banks, "Id", "Name", bankAccount.BankId);
            ViewBag.BankId = new SelectList(currentUserBanks, "Id", "Name", bankAccount.BankId);
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseholdId);
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            if (bankAccount.Closed != null)
            {
                return View("Error");
            }

            if (bankAccount.Balance == 0)
            {
                return View(bankAccount);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankAccount = db.BankAccounts.Find(id);
            bankAccount.Closed = DateTimeOffset.UtcNow;
            //db.BankAccounts.Remove(bankAccount);
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

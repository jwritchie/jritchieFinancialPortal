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
using jritchieFinancialPortal.Models.Helpers;
using Microsoft.AspNet.Identity;

namespace jritchieFinancialPortal.Controllers
{
    [AuthorizeHouseholdRequired]
    public class TransactionsController : UniversalController
    {
        // GET: Transactions
        public ActionResult Index()
        {
            var currentUserHousehold = User.Identity.GetHouseholdId();
            List<Transaction> currentUserTransactions = new List<Transaction>();
            currentUserTransactions = db.Transactions.Where(t => t.Account.HouseholdId == currentUserHousehold).ToList();

            return View(currentUserTransactions);

            //var transactions = db.Transactions.Include(t => t.Account).Include(t => t.Category).Include(t => t.PostedBy).Include(t => t.ReconciledBy);
            //return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            var userHouseholdId = User.Identity.GetHouseholdId();
            List<BankAccount> currentUserBankAccounts = new List<BankAccount>();
            currentUserBankAccounts = db.BankAccounts.Where(b => b.HouseholdId == userHouseholdId && b.Closed == null).OrderBy(b => b.Name).ToList();
            ViewBag.AccountId = new SelectList(currentUserBankAccounts, "Id", "Name");

            List<TransactionType> transactionTypes = new List<TransactionType>();
            transactionTypes = db.TransactionTypes.ToList();
            ViewBag.TransactionTypeId = new SelectList(transactionTypes, "Id", "Name");

            //List<Category> currentUserCategories = new List<Category>();
            //currentUserCategories = db.Categories.Where(c => c.HouseholdId == userHouseholdId).OrderBy(c => c.Name).ToList();
            //ViewBag.CategoryId = new SelectList(currentUserCategories, "Id", "Name");

            //ViewBag.PostedById = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.ReconciledById = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,PostedById,DatePosted,Amount,Description,CategoryId,Reconciled,ReconciledById,DateReconciled,Void,DateOfTransaction")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.DatePosted = DateTimeOffset.UtcNow;
                transaction.PostedById = User.Identity.GetUserId();

                db.Transactions.Add(transaction);
                db.SaveChanges();

                var updatedBalance = db.Transactions.Where(t => t.Void == false && t.AccountId == transaction.AccountId).Sum(t => t.Amount);
                var accountToUpdate = db.BankAccounts.Find(transaction.AccountId);
                accountToUpdate.Balance = updatedBalance;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            var userHouseholdId = User.Identity.GetHouseholdId();
            List<BankAccount> currentUserBankAccounts = new List<BankAccount>();
            currentUserBankAccounts = db.BankAccounts.Where(b => b.HouseholdId == userHouseholdId && b.Closed == null).OrderBy(b => b.Name).ToList();
            ViewBag.AccountId = new SelectList(currentUserBankAccounts, "Id", "Name", transaction.AccountId);

            List<TransactionType> transactionTypes = new List<TransactionType>();
            transactionTypes = db.TransactionTypes.ToList();
            ViewBag.TransactionTypeId = new SelectList(transactionTypes, "Id", "Name");

            //List<Category> currentUserCategories = new List<Category>();
            //currentUserCategories = db.Categories.Where(c => c.HouseholdId == userHouseholdId).OrderBy(c => c.Name).ToList();
            //ViewBag.CategoryId = new SelectList(currentUserCategories, "Id", "Name", transaction.CategoryId);

            //ViewBag.PostedById = new SelectList(db.Users, "Id", "FirstName", transaction.PostedById);
            //ViewBag.ReconciledById = new SelectList(db.Users, "Id", "FirstName", transaction.ReconciledById);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }

            var userHouseholdId = User.Identity.GetHouseholdId();
            List<BankAccount> currentUserBankAccounts = new List<BankAccount>();
            currentUserBankAccounts = db.BankAccounts.Where(b => b.HouseholdId == userHouseholdId && b.Closed == null).OrderBy(b => b.Name).ToList();
            ViewBag.AccountId = new SelectList(currentUserBankAccounts, "Id", "Name", transaction.AccountId);

            List<TransactionType> transactionTypes = new List<TransactionType>();
            transactionTypes = db.TransactionTypes.ToList();
            ViewBag.TransactionTypeId = new SelectList(transactionTypes, "Id", "Name");


            List<Category> currentUserCategories = new List<Category>();
            currentUserCategories = db.Categories.Where(c => c.HouseholdId == userHouseholdId).Where(c => c.TransactionTypeId == transaction.Category.TransactionTypeId).OrderBy(c => c.Name).ToList();
            ViewBag.CategoryId = new SelectList(currentUserCategories, "Id", "Name", transaction.CategoryId);

            ViewBag.TransactionsAccountId = transaction.AccountId;
            
            //ViewBag.PostedById = new SelectList(db.Users, "Id", "FirstName", transaction.PostedById);
            //ViewBag.ReconciledById = new SelectList(db.Users, "Id", "FirstName", transaction.ReconciledById);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,PostedById,DatePosted,Amount,Description,CategoryId,Reconciled,ReconciledById,DateReconciled,Void,DateOfTransaction")] Transaction transaction, int PriorAccountId)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();

                var updatedBalance = db.Transactions.Where(t => t.Void == false && t.AccountId == transaction.AccountId).Sum(t => (decimal?)t.Amount) ?? 0;
                var accountToUpdate = db.BankAccounts.Find(transaction.AccountId);
                accountToUpdate.Balance = updatedBalance;
                db.SaveChanges();

                if (PriorAccountId != transaction.AccountId)
                {
                    var priorAccountUpdatedBalance = db.Transactions.Where(t => t.Void == false && t.AccountId == PriorAccountId).Sum(t => (decimal?)t.Amount) ?? 0;
                    var priorAccountAccountToUpdate = db.BankAccounts.Find(PriorAccountId);
                    priorAccountAccountToUpdate.Balance = priorAccountUpdatedBalance;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            var userHouseholdId = User.Identity.GetHouseholdId();
            List<BankAccount> currentUserBankAccounts = new List<BankAccount>();
            currentUserBankAccounts = db.BankAccounts.Where(b => b.HouseholdId == userHouseholdId && b.Closed == null).OrderBy(b => b.Name).ToList();
            ViewBag.AccountId = new SelectList(currentUserBankAccounts, "Id", "Name", transaction.AccountId);

            List<TransactionType> transactionTypes = new List<TransactionType>();
            transactionTypes = db.TransactionTypes.ToList();
            ViewBag.TransactionTypeId = new SelectList(transactionTypes, "Id", "Name");

            //List<Category> currentUserCategories = new List<Category>();
            //currentUserCategories = db.Categories.Where(c => c.HouseholdId == userHouseholdId).OrderBy(c => c.Name).ToList();
            //ViewBag.CategoryId = new SelectList(currentUserCategories, "Id", "Name", transaction.CategoryId);

            //ViewBag.PostedById = new SelectList(db.Users, "Id", "FirstName", transaction.PostedById);
            //ViewBag.ReconciledById = new SelectList(db.Users, "Id", "FirstName", transaction.ReconciledById);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Transaction transaction = db.Transactions.Find(id);
        //    if (transaction == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(transaction);
        //}

        // POST: Transactions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Transaction transaction = db.Transactions.Find(id);
        //    db.Transactions.Remove(transaction);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}



        // ActionResult for Ajax call
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCategoryByType(int typeId)
        {
            var userHouseholdId = User.Identity.GetHouseholdId();
            List<Category> currentUserCategories = new List<Category>();
            currentUserCategories = db.Categories.Where(c => c.HouseholdId == userHouseholdId).Where(c => c.TransactionTypeId == typeId).OrderBy(c => c.Name).ToList();
            //ViewBag.CategoryId = new SelectList(currentUserCategories, "Id", "Name");
            SelectList selectedCategories = new SelectList(currentUserCategories, "Id", "Name", 0);
            return Json(selectedCategories);
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

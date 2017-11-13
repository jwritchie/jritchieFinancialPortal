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

namespace jritchieFinancialPortal.Controllers
{
    [AuthorizeHouseholdRequired]
    public class BudgetsController : UniversalController
    {

        // GET: Budgets
        public ActionResult Index()
        {
            var currentHouseholdId = User.Identity.GetHouseholdId();
            var budgets = db.Budgets.Where(b => b.HouseholdId == currentHouseholdId);
            //var budgets = db.Budgets.Include(b => b.Category).Include(b => b.Frequency).Include(b => b.Household).Include(b => b.TransactionType);
            return View(budgets.ToList());
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }

            // Build & populate BudgetTransactionsViewModel
            BudgetTransactionsViewModel btViewModel = new BudgetTransactionsViewModel();
            btViewModel.Budget = budget;
            int currentUserHouseholdId = User.Identity.GetHouseholdId().Value;
            btViewModel.Transactions = db.Transactions.Where(t => t.Account.HouseholdId == currentUserHouseholdId).Where(t => t.CategoryId == budget.CategoryId).ToList();
            decimal currentTransactionsTotal = db.Transactions.Where(t => t.Account.HouseholdId == currentUserHouseholdId).Where(t => t.CategoryId == budget.CategoryId).Sum(t => (decimal?)t.Amount) ?? 0;
            btViewModel.TotalTransactions = currentTransactionsTotal * -1;
            return View(btViewModel);

            //return View(budget);
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            //ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name");
            ViewBag.FrequencyId = new SelectList(db.Frequencies, "Id", "Name");
            return View();

            //Budget budget = new Budget();
            //budget.HouseholdId = ViewBag.CurrentUserHouseholdId;
            //return View(budget);
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Amount,CategoryId,TransactionTypeId,FrequencyId")] Budget budget)
        //public ActionResult Create([Bind(Include = "Id,Name,Amount,DateCreated,DateUpdated,CategoryId,HouseholdId,TransactionTypeId,FrequencyId")] Budget budget)
        {
            //budget.TransactionTypeId = db.TransactionTypes.First(t => t.Id == budget.Category.TransactionTypeId );

            if (ModelState.IsValid)
            {
                //var currentHouseholdId = User.Identity.GetHouseholdId();
                //budget.HouseholdId = currentHouseholdId.GetValueOrDefault();

                budget.HouseholdId = ViewBag.CurrentUserHouseholdId;
                budget.DateCreated = DateTimeOffset.UtcNow;

                //int currentTransactionType = db.Categories.Find(budget.CategoryId).TransactionTypeId.Value;
                budget.TransactionTypeId = db.Categories.Find(budget.CategoryId).TransactionTypeId.Value;

                //int currentTransactionType = db.TransactionTypes.FirstOrDefault(t => t.Name == budget.Category.Name).Id;

                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", budget.CategoryId);
            ViewBag.FrequencyId = new SelectList(db.Frequencies, "Id", "Name", budget.FrequencyId);
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name", budget.TransactionTypeId);
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }

            var userHouseholdId = User.Identity.GetHouseholdId();
            List<Category> currentUserCategories = new List<Category>();
            currentUserCategories = db.Categories.Where(c => c.HouseholdId == userHouseholdId).Where(c => c.TransactionTypeId == budget.Category.TransactionTypeId).OrderBy(c => c.Name).ToList();
            ViewBag.CategoryId = new SelectList(currentUserCategories, "Id", "Name", budget.CategoryId);

            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", budget.CategoryId);
            ViewBag.FrequencyId = new SelectList(db.Frequencies, "Id", "Name", budget.FrequencyId);
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name", budget.TransactionTypeId);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Amount,DateCreated,DateUpdated,CategoryId,HouseholdId,TransactionTypeId,FrequencyId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                budget.TransactionTypeId = db.Categories.Find(budget.CategoryId).TransactionTypeId.Value;
                budget.DateUpdated = DateTimeOffset.UtcNow;

                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var userHouseholdId = User.Identity.GetHouseholdId();
            List<Category> currentUserCategories = new List<Category>();
            currentUserCategories = db.Categories.Where(c => c.HouseholdId == userHouseholdId).Where(c => c.TransactionTypeId == budget.Category.TransactionTypeId).OrderBy(c => c.Name).ToList();
            ViewBag.CategoryId = new SelectList(currentUserCategories, "Id", "Name", budget.CategoryId);

            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", budget.CategoryId);
            ViewBag.FrequencyId = new SelectList(db.Frequencies, "Id", "Name", budget.FrequencyId);
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name", budget.TransactionTypeId);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budget budget = db.Budgets.Find(id);
            db.Budgets.Remove(budget);
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

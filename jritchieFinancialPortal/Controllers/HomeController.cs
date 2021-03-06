﻿using jritchieFinancialPortal.Models;
using jritchieFinancialPortal.Models.CodeFirst;
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
    [RequireHttps]
    [Authorize]
    public class HomeController : UniversalController
    {
        [AllowAnonymous]
        public ActionResult LandingPage()
        {
            return View();
        }


        //[AuthorizeHouseholdRequired]
        //public ActionResult Index()
        //{
        //    var currentUserHousehold = User.Identity.GetHouseholdId();
        //    List<Transaction> currentUserTransactions = new List<Transaction>();
        //    currentUserTransactions = db.Transactions.Where(t => t.Account.HouseholdId == currentUserHousehold).ToList();

        //    return View(currentUserTransactions);
        //}


        [AuthorizeHouseholdRequired]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AuthorizeHouseholdRequired]
        // underscore denotes a partial view.
        public PartialViewResult _Contact(string id)
        {
            ViewBag.Message = id;

            return PartialView();
        }


        // GET: DashBoard/Index
        [AuthorizeHouseholdRequired]
        public ActionResult Index()
        {
            // Build & populate DashTransactionsViewModel
            List<DashTransactionsViewModel> dashViewModel = new List<DashTransactionsViewModel>();

            int currentUserHouseholdId = User.Identity.GetHouseholdId().Value;
            int expenseTypeId = db.TransactionTypes.First(t => t.Name == "Expense").Id;
            int incomeTypeId = db.TransactionTypes.First(t => t.Name == "Income").Id;

            List<BankAccount> currentUserBankAccounts = db.BankAccounts.Where(b => b.HouseholdId == currentUserHouseholdId).ToList();

            int currentYear = DateTimeOffset.UtcNow.Year;


            foreach(var account in currentUserBankAccounts)
            {
                DashTransactionsViewModel currentAccountVM = new DashTransactionsViewModel();

                decimal currentExpenseTotal = account.Transactions.Where(t => t.DateOfTransaction.Year == currentYear).Where(t => t.Category.TransactionTypeId == expenseTypeId).Sum(t => (decimal?)t.Amount) ?? 0;
                decimal currentIncomeTotal = account.Transactions.Where(t => t.DateOfTransaction.Year == currentYear).Where(t => t.Category.TransactionTypeId == incomeTypeId).Sum(t => (decimal?)t.Amount) ?? 0;

                currentAccountVM.AccountName = account.Name;
                currentAccountVM.TotalExpenseTransactions = currentExpenseTotal * -1;
                currentAccountVM.TotalIncomeTransactions = currentIncomeTotal;

                List<Transaction> currentAccountTransactions = new List<Transaction>();
                currentAccountTransactions = db.Transactions.Where(t => t.DateOfTransaction.Year == currentYear).Where(t => t.Account.HouseholdId == currentUserHouseholdId).Where(t =>t.AccountId == account.Id).OrderByDescending(t => t.DateOfTransaction).Take(10).ToList();
                currentAccountVM.Transaction = currentAccountTransactions;

                currentAccountVM.CurrentYear = currentYear;

                if (account.Balance < 0)
                {
                    currentAccountVM.AccountOverdrawn = true;
                }
                else
                {
                    currentAccountVM.AccountOverdrawn = false;
                }

                dashViewModel.Add(currentAccountVM);
            }

            List<BudgetTransactionsViewModel> btVM = new List<BudgetTransactionsViewModel>();
            List<Budget> currentUserBudgets = db.Budgets.Where(b => b.HouseholdId == currentUserHouseholdId).ToList();
            foreach (var budget in currentUserBudgets)
            {
                // Build & populate BudgetTransactionsViewModel
                BudgetTransactionsViewModel btViewModel = new BudgetTransactionsViewModel();
                btViewModel.Budget = budget;
                btViewModel.Transactions = db.Transactions.Where(t => t.Account.HouseholdId == currentUserHouseholdId).Where(t => t.CategoryId == budget.CategoryId).ToList();
                decimal currentTransactionsTotal = db.Transactions.Where(t => t.Account.HouseholdId == currentUserHouseholdId).Where(t => t.CategoryId == budget.CategoryId).Sum(t => (decimal?)t.Amount) ?? 0;
                btViewModel.TotalTransactions = currentTransactionsTotal * -1;
                btViewModel.DisplayTotalTransactions = currentTransactionsTotal;

                btVM.Add(btViewModel);
            }

            ViewBag.DashBudgets = btVM;

            return View(dashViewModel);
        }

    }
}
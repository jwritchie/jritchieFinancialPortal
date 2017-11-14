using jritchieFinancialPortal.Models;
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

            foreach(var account in currentUserBankAccounts)
            {
                DashTransactionsViewModel currentAccountVM = new DashTransactionsViewModel();

                decimal currentExpenseTotal = account.Transactions.Where(t => t.Category.TransactionTypeId == expenseTypeId).Sum(t => (decimal?)t.Amount) ?? 0;
                decimal currentIncomeTotal = account.Transactions.Where(t => t.Category.TransactionTypeId == incomeTypeId).Sum(t => (decimal?)t.Amount) ?? 0;

                currentAccountVM.AccountName = account.Name;
                currentAccountVM.TotalExpenseTransactions = currentExpenseTotal * -1;
                currentAccountVM.TotalIncomeTransactions = currentIncomeTotal;

                dashViewModel.Add(currentAccountVM);
            }

            return View(dashViewModel);


            //decimal currentExpenseTransactionsTotal = db.Transactions.Where(t => t.Account.HouseholdId == currentUserHouseholdId).Where(t => t.Category.TransactionTypeId == 1).Sum(t => (decimal?)t.Amount) ?? 0;
            //decimal currentIncomeTransactionsTotal = db.Transactions.Where(t => t.Account.HouseholdId == currentUserHouseholdId).Where(t => t.Category.TransactionTypeId == 2).Sum(t => (decimal?)t.Amount) ?? 0;

            //dashViewModel.TotalExpenseTransactions = currentIncomeTransactionsTotal * -1;
            //dashViewModel.TotalIncomeTransactions = currentIncomeTransactionsTotal;
            //return View(dashViewModel);
        }

    }
}
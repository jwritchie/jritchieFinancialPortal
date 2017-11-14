using jritchieFinancialPortal.Models.CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models
{
    public class DashTransactionsViewModel
    {
        //public List<Transaction> IncomeTransactions { get; set; }
        //public List<Transaction> ExpenseTransactions { get; set; }
        public string AccountName { get; set; }
        public decimal TotalIncomeTransactions { get; set; }
        public decimal TotalExpenseTransactions { get; set; }
    }
}
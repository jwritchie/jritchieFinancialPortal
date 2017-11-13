using jritchieFinancialPortal.Models.CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models
{
    public class BudgetTransactionsViewModel
    {
        public Budget Budget { get; set; }
        public List<Transaction> Transactions { get; set; }
        public decimal TotalTransactions { get; set; }
    }
}
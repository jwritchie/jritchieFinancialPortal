using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    public class Expense : IBudgetItem, ICalculate
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string DescriptionName { get; set; }
        public int BudgetId { get; set; }
        public int Frequency { get; set; }

        public virtual Budget Budget { get; set; }

        public decimal CalculateYearlyTotal()
        {
            return Amount * Frequency;
        }
    }
}
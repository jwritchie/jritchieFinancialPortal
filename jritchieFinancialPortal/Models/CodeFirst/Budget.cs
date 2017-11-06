using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    public class Budget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int CategoryID { get; set; }
        public int HouseholdId { get; set; }

        //public virtual Category Category { get; set; }
        public virtual Household Household { get; set; }

        public virtual ICollection<Income> Incomes { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
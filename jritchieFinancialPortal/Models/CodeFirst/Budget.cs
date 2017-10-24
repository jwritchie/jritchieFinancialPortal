using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    public class Budget
    {
        int Id { get; set; }
        decimal Amount { get; set; }
        // Category
        // Duration
        // Name

        public virtual ICollection<Income> Incomes { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<Household> Households { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

    }
}
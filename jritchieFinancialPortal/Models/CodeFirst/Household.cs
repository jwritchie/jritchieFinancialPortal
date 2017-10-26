using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    public class Household
    {
        public Household()
        {
            // HashSet for faster access to data.
            Users = new HashSet<ApplicationUser>();     // Assign multiple users to household.
            Accounts = new HashSet<BankAccount>();      // Assign multiple accounts.
            Budgets = new HashSet<Budget>();            // Assign multiple budgets.
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTimeOffset Established { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<BankAccount> Accounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
    }
}
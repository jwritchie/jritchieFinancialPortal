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
            Accounts = new HashSet<Account>();          // Assign multiple accounts.
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTimeOffset Established { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }

    }
}
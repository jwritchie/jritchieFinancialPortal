using jritchieFinancialPortal.Models.CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models
{
    public class HouseholdUserViewModel
    {
        public Household Household { get; set; }
        public List<ApplicationUser> SelectedUsers { get; set; }
        public string[] SelectedUsersName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    public class Invitation
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        [Required]
        public string Email { get; set; }
        public string PasswordGUID { get; set; }
        public DateTimeOffset DateTimeIssued { get; set; }
        public DateTimeOffset DateTimeExpires { get; set; }

        public virtual Household Household { get; set; }
    }
}
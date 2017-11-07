using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string PostedById { get; set; }
        public DateTimeOffset DatePosted { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public bool Reconciled { get; set; }
        public string ReconciledById { get; set; }
        public DateTimeOffset? DateReconciled { get; set; }
        public bool Void { get; set; }
        public DateTimeOffset DateOfTransaction { get; set; }

        public virtual BankAccount Account { get; set; }
        public virtual ApplicationUser PostedBy { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser ReconciledBy { get; set; }
    }
}
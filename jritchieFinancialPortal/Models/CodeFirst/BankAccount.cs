﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    public class BankAccount
    {
        public BankAccount()
        {
            // HashSets for faster access to data.
            Users = new HashSet<ApplicationUser> ();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public decimal Balance { get; set; }
        public decimal BalanceReconciled { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Opened { get; set; }
        public DateTimeOffset? Closed { get; set; }
        public int? HouseholdId { get; set; }
        public int BankId { get; set; }

        public virtual Household Household { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
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
        public decimal Amount { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }

        public int CategoryId { get; set; }
        public int HouseholdId { get; set; }
        public int TransactionTypeId { get; set; }
        public int FrequencyId { get; set; }


        public virtual Category Category { get; set; }
        public virtual Household Household { get; set; }
        public virtual TransactionType TransactionType { get; set; }
        public virtual Frequency Frequency { get; set; }


    }
}
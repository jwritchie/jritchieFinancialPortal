using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    public class Category
    {
        // Lookup table.
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TransactionTypeId { get; set; }
        public int? HouseholdId { get; set; }

        public virtual TransactionType TransactionType { get; set; }
        public virtual Household Household {get; set;}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    public class Income : IBudgetItem
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string DescriptionName { get; set; }
        public Category CategoryId { get; set; }
        public int Frequency { get; set; }

        public virtual Category Category { get; set; }
    }
}
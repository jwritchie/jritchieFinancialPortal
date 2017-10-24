using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    public class Income : IBudgetItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Date { get; set; }
        public int Frequency { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
    }
}
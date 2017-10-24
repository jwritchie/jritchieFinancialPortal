using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    interface IBudgetItem
    {
        string Name { get; set; }
         DateTimeOffset Date { get; set; }
        int Frequency { get; set; }
        string Type { get; set; }
        decimal Amount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    interface IBudgetItem
    {
        int Id { get; set; }
        decimal Amount { get; set; }
        string DescriptionName { get; set; }
        Category CategoryId { get; set; }
        int Frequency { get; set; }

        Category Category { get; set; }

    }
}

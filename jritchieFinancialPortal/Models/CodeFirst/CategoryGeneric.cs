using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    public class CategoryGeneric
    {
        // Lookup table.
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
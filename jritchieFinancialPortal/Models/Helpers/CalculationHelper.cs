using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jritchieFinancialPortal.Models.CodeFirst
{
    public class CalculationHelper
    {
        private ICalculate _calculate;

        public CalculationHelper(ICalculate calculate)
        {
            this._calculate = calculate;
        }

        public decimal CalculateYearly()
        {
            return _calculate.CalculateYearlyTotal();
        }
    }
}



/*
EXAMPLE

Controller:

public ActionResult Index()
{
CalculationHelper helper = new CalcuationHelper(new Expense());
ViewBag.Result = helper.CalculateYearly();
return View();
}

*/
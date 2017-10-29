using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace jritchieFinancialPortal.Models.Helpers
{
    public static class Extensions
    {
        public static int? GetHouseholdId(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var HouseholdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "HouseholdId");

            if(HouseholdClaim != null)
            {
                return Int32.Parse(HouseholdClaim.Value);
            }
            else
            {
                return null;
            }
        }

        public static bool IsInHousehold(this IIdentity user)
        {
            var claimsUser = (ClaimsIdentity)user;
            var householdId = claimsUser.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
            return (householdId != null && !string.IsNullOrWhiteSpace(householdId.Value));
        }
    }
}
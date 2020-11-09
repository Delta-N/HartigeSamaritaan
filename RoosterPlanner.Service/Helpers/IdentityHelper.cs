using System.Linq;
using System.Security.Claims;

namespace RoosterPlanner.Service.Helpers
{
    public class IdentityHelper
    {
        public static string GetOid(ClaimsIdentity claimsIdentity)
        {
            ClaimsIdentity identity = claimsIdentity;
            string oid = null;
            if (identity != null)
                oid = identity.Claims.FirstOrDefault(c =>
                        c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                    ?.Value;
            return oid;
        }
    }
}
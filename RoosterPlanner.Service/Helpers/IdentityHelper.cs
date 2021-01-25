using System.Linq;
using System.Security.Claims;

namespace RoosterPlanner.Service.Helpers
{
    public static class IdentityHelper
    {   
        /// <summary>
        /// Get the OID of the user from the JWT token (claimsIdentity)
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <returns></returns>
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
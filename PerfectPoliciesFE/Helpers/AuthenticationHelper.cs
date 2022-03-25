using Microsoft.AspNetCore.Http;
using System.Linq;

namespace PerfectPoliciesFE.Helpers
{
    public static class AuthenticationHelper
    {
        public static bool isAuthenticated(HttpContext context)
        {
            return context.Session.Keys.Any(c => c.Equals("Token"));
        }
    }
}

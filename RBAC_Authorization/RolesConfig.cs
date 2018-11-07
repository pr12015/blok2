using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBAC_Authorization
{
    public class RolesConfig
    {
        private static List<string> AdminPermissions = new List<string>()
        {
            Permissions.Delete.ToString(),
            Permissions.Add.ToString()
        };

        private static List<string> PotrosacPermissions = new List<string>()
        {
            Permissions.Read.ToString()
        };

        private static List<string> SuperKorisnikPermissions = new List<string>()
        {
            Permissions.DeleteAll.ToString()
        };

        private static List<string> OperatorPermissions = new List<string>()
        {
            Permissions.Modify.ToString()
        };

        private static List<string> NoPermissions = new List<string>();

        public static List<string> GetPermissions(string role)
        {
            switch (role)
            {
                case "Potrosac": return PotrosacPermissions;
                case "Operator": return OperatorPermissions;
                case "Admin": return AdminPermissions;
                case "SuperKorisnik": return SuperKorisnikPermissions;
                default: return NoPermissions; // ili null
            }
        }
    }
}

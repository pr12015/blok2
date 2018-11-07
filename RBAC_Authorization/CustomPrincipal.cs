using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace RBAC_Authorization
{
    public class CustomPrincipal : IPrincipal
    {
        private WindowsIdentity winId = null;
        private Dictionary<string, List<string>> Roles = new Dictionary<string, List<string>>();

        public IIdentity Identity
        {
            get { return this.winId; }
        }

        public CustomPrincipal(WindowsIdentity winIdentity)
        {
            this.winId = winIdentity;

            // ubacimo grupe sa dozvolama
            foreach (var group in this.winId.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));

                // ovde bila greska, nigde veze koja glupost
                string groupName = string.Empty;
                if (name.ToString().Contains("\\"))
                    groupName = name.ToString().Split('\\')[1];

                if (!Roles.ContainsKey(groupName))
                {
                    Roles.Add(groupName, RolesConfig.GetPermissions(groupName));
                }
            }
        }

        public bool IsInRole(string role)
        {
            foreach (var userPermissions in Roles.Values)
                if (userPermissions.Contains(role))
                    return true;

            return false;
        }
    }
}

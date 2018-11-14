using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using EventLogger;

namespace RBAC_Authorization
{
    public class CustomPrincipal : IPrincipal
    {
        private WindowsIdentity winId = null;
        private List<string> Roles = new List<string>();

        public IIdentity Identity
        {
            get { return this.winId; }
        }

        public CustomPrincipal(WindowsIdentity winIdentity)
        {
            this.winId = winIdentity;

            /// Add groups with permissions.
            foreach (var group in this.winId.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));

                string groupName = string.Empty;
                if (name.ToString().Contains("\\"))
                    groupName = name.ToString().Split('\\')[1];

                if (!Roles.Contains(groupName))
                {
                    Roles.Add(groupName);
                }
            }
        }

        public bool IsInRole(string role)
        {
            foreach (var group in Roles)
            {
                var permissions = RBACConfigParser.GetPermissions(group);
                if (permissions.Contains(role))
                    return true;
            }

            return false;
        }
    }
}

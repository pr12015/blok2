using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.ServiceModel;

namespace RBAC_Authorization
{
    public class CustomAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            IPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IPrincipal;

            if (principal != null)
            {
                string permission = Permissions.Read.ToString().ToLower();
                return (principal as CustomPrincipal).IsInRole(permission);
            }

            return false;
        }
    }
}

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
            EventLogger.Logger logger = new EventLogger.Logger("Service.Audit", "ServiceLog");

            if (principal != null)
            {
                string permission = Permissions.Read.ToString().ToLower();
                var success = (principal as CustomPrincipal).IsInRole(permission);
                if (success)
                {
                    logger.Log(principal.Identity.Name, "Read", "", EventLogger.EventType.AuthenticationSuccess);
                }
                else
                {
                    logger.Log(principal.Identity.Name, "Read", permission, EventLogger.EventType.AuthenticationFailure);
                }
            }

            return false;
        }
    }
}

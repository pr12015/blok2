using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBAC_Authorization
{
    public enum Permissions
    {
        Add,
        Modify,
        Delete,
        DeleteAll,
        Read
    }

    public enum Roles
    {
        Customer,
        Operator,
        Admin,
        SuperUser
    }
}

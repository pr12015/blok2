using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using System.Threading;
using RBAC_Authorization;

namespace Service
{
    class SmartMeterService : ISmartMeterService
    {
        private static int value = 10;

        public bool ModifyDB(int newValue)
        {
            bool modified = false;

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            /// audit both successfull and failed authorization checks
            if (principal.IsInRole(Permissions.Modify.ToString()))
            {
                value = newValue;
                Console.WriteLine("ModifyDB() passed for user {0}.", principal.Identity.Name);
                modified = true;
            }
            else
            {
                Console.WriteLine("ModifyDB() failed for user {0}.", principal.Identity.Name);
            }

            return modified;
        }

        public bool AddDB()
        {
            bool deleted = false;

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            /// audit both successfull and failed authorization checks

            if (principal.IsInRole(Permissions.Add.ToString()))
            {
                value = 0;
                Console.WriteLine("AddDB() passed for user {0}.", principal.Identity.Name);
                deleted = true;
            }
            else
            {
                Console.WriteLine("AddDB() failed for user {0}.", principal.Identity.Name);
            }

            return deleted;
        }

        public bool DeleteEntityDB()
        {
            bool deleted = false;

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            /// audit both successfull and failed authorization checks

            if (principal.IsInRole(Permissions.Delete.ToString()))
            {
                value = 0;
                Console.WriteLine("DeleteEntityDB() passed for user {0}.", principal.Identity.Name);
                deleted = true;
            }
            else
            {
                Console.WriteLine("DeleteEntityDB() failed for user {0}.", principal.Identity.Name);
            }

            return deleted;
        }

        public bool DeleteDB()
        {
            bool deleted = false;

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            /// audit both successfull and failed authorization checks

            if (principal.IsInRole(Permissions.DeleteAll.ToString()))
            {
                value = 0;
                Console.WriteLine("DeleteDB() passed for user {0}.", principal.Identity.Name);
                deleted = true;
            }
            else
            {
                Console.WriteLine("DeleteDB() failed for user {0}.", principal.Identity.Name);
            }

            return deleted;
        }

        public int GetBill()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            Console.WriteLine("Read() successfully executed by {0}.", principal.Identity.Name);
            return value;
        }

    }
}

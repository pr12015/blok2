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
        public bool ModifyID(int newValue, int oldValue)
        {
            bool modified = false;

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            var permission = Permissions.Modify.ToString().ToLower();
            /// audit both successfull and failed authorization checks
            if (principal.IsInRole(permission))
            {
                Console.WriteLine("ModifyDB() passed for user {0}.", principal.Identity.Name);
                DataBase.ModifyID(oldValue, newValue);
                modified = true;
            }
            else
            {
                Console.WriteLine("ModifyDB() failed for user {0}.", principal.Identity.Name);
            }

            return modified;
        }

        public bool ModifyReading(double newValue, int id)
        {
            bool modified = false;

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            var permission = Permissions.Modify.ToString().ToLower();
            /// audit both successfull and failed authorization checks
            if (principal.IsInRole(permission))
            {
                Console.WriteLine("ModifyDB() passed for user {0}.", principal.Identity.Name);
                DataBase.ModifyReading(id, newValue);
                modified = true;
            }
            else
            {
                Console.WriteLine("ModifyDB() failed for user {0}.", principal.Identity.Name);
            }

            return modified;
        }

        public bool AddDB(int id, string fullName, double reading)
        {
            bool added = false;
            EMeter newEMeter = new EMeter(id, fullName, reading); 
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            var permission = Permissions.Add.ToString().ToLower();
            /// audit both successfull and failed authorization checks

            if (principal.IsInRole(permission))
            {
                Console.WriteLine("AddDB() passed for user {0}.", principal.Identity.Name);
                DataBase.Write(newEMeter);
                added = true;
            }
            else
            {
                Console.WriteLine("AddDB() failed for user {0}.", principal.Identity.Name);
            }

            return added;
        }

        public bool DeleteEntityDB(int id)
        {
            bool deleted = false;

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            var permission = Permissions.Delete.ToString().ToLower();

            /// audit both successfull and failed authorization checks
            if (principal.IsInRole(permission))
            {
                Console.WriteLine("DeleteEntityDB() passed for user {0}.", principal.Identity.Name);
                DataBase.DeleteEntity(id);
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

            var permission = Permissions.DeleteAll.ToString().ToLower();
            
            /// audit both successfull and failed authorization checks
            if (principal.IsInRole(permission))
            {
                Console.WriteLine("DeleteDB() passed for user {0}.", principal.Identity.Name);
                DataBase.DeleteDB();
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
           // if (principal.IsInRole(Permissions.Read.ToString()))
           // {
                Console.WriteLine("Read() successfully executed by {0}.", principal.Identity.Name);
                return 1;
           // }
           /* else
            {
                Console.WriteLine("GetBill() failed for user {0}.", principal.Identity.Name);
            }
            return -1;
            */
        }

    }
}
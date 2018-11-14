using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using System.Threading;
using RBAC_Authorization;
using System.ServiceModel;
using EventLogger;

namespace Service
{
    class SmartMeterService : ISmartMeterService
    {
        
        Logger logger = new Logger("Service.Audit", "ServiceLog");
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
                logger.Log(principal.Identity.Name, "ModifyID", "", EventType.AuthenticationSuccess);
            }
            else
            {
                logger.Log(principal.Identity.Name, "ModifyID", permission, EventType.AuthenticationFailure);
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
                logger.Log(principal.Identity.Name, "ModifyReading", "", EventType.AuthenticationSuccess);
            }
            else
            {
                logger.Log(principal.Identity.Name, "ModifyReading", permission, EventType.AuthenticationFailure);
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
                logger.Log(principal.Identity.Name, "AddDB", "", EventType.AuthenticationSuccess);
            }
            else
            {
                logger.Log(principal.Identity.Name, "AddDB", permission, EventType.AuthenticationFailure);
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
                logger.Log(principal.Identity.Name, "DeleteEntityDB", "", EventType.AuthenticationSuccess);
            }
            else
            {
                logger.Log(principal.Identity.Name, "DeleteEntityDB", permission, EventType.AuthenticationFailure);
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
                logger.Log(principal.Identity.Name, "DeleteDB", "", EventType.AuthenticationSuccess);
            }
            else
            {
                logger.Log(principal.Identity.Name, "DeleteDB", permission, EventType.AuthenticationFailure);
                Console.WriteLine("DeleteDB() failed for user {0}.", principal.Identity.Name);
            }

            return deleted;
        }

        public double GetBill(int id)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            Console.WriteLine("Read() successfully executed by {0}.", principal.Identity.Name);
            var eMeter = DataBase.Read(id);

            var binding = new NetTcpBinding();
            string addressLB = "net.tcp://localhost:9997/LBDuplex";
            using (LoadBalancerClient proxy = new LoadBalancerClient(binding, new EndpointAddress(addressLB)))
            {
                return proxy.RequestBill(eMeter.Reading);
            }
        }

    }
}
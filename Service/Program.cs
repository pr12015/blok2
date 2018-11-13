using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RBAC_Authorization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.IdentityModel.Policy;
using Contracts;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/WCFService";

            ServiceHost host = new ServiceHost(typeof(SmartMeterService));
            host.AddServiceEndpoint(typeof(ISmartMeterService), binding, address);

            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

            // Add a custom authorization policy to the service authorization behavior.
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;

            host.Open();
            Console.WriteLine("WCFService is opened. Press <enter> to finish...");

            string addressLB = "net.tcp://localhost:9997/LBDuplex";
            using (LoadBalancerClient proxy = new LoadBalancerClient(binding, new EndpointAddress(addressLB)))
            {
                Console.WriteLine(proxy.RequestBill(12.11));
            }
            
            Console.ReadLine();
            host.Close();
        }
    }
}

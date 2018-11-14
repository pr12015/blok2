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
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

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

            Console.ReadLine();
            host.Close();
        }
    }
}

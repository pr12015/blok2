using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CertificateManager;
using System.Security.Principal;
using System.IdentityModel;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;

namespace LoadBalancer
{
    class Program
    {
        static void Main(string[] args)
        {
            string serviceCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            var bindingWorker = new NetTcpBinding();
            bindingWorker.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string addressWorker = "net.tcp://localhost:9998/LBDuplexWorker";
            string addressServer = "net.tcp://localhost:9997/LBDuplex";

            //Console.ReadKey(false);

            var hostWorker = new ServiceHost(typeof(LBDuplex));
            hostWorker.AddServiceEndpoint(typeof(IWorker), bindingWorker, addressWorker);

            hostWorker.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            hostWorker.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            hostWorker.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, serviceCertCN);

            var bindingServer = new NetTcpBinding();
            var hostServer = new ServiceHost(typeof(LBDuplex));
            hostServer.AddServiceEndpoint(typeof(IServer), bindingServer, addressServer);
            ServiceDebugBehavior debug = hostServer.Description.Behaviors.Find<ServiceDebugBehavior>();
            CallbackDebugBehavior debugWorker = hostServer.Description.Behaviors.Find<CallbackDebugBehavior>();

            if(debug == null || debugWorker == null)
            {
                //hostServer.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
                //hostWorker.Description.Behaviors.Add(new CallbackDebugBehavior(true));
            }
            else
            {
                if (!debug.IncludeExceptionDetailInFaults || !debugWorker.IncludeExceptionDetailInFaults)
                {
                    debug.IncludeExceptionDetailInFaults = true;
                    //debugWorker.IncludeExceptionDetailInFaults = true;
                }
            }

            try
            {
                hostServer.Open();
                Console.WriteLine("LB Server service is open. Press <enter> to finish...");
                hostWorker.Open();
                Console.WriteLine("LB Worker service is open. Press <enter> to finish...");
            }
            catch(Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }

            Console.WriteLine("LB WCF service is open. Press <enter> to finish...");
            Console.ReadKey(false);

            hostServer.Close();
            hostWorker.Close();
        }
    }
}

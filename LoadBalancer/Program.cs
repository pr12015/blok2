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

namespace LoadBalancer
{
    class Program
    {
        static void Main(string[] args)
        {
            string serviceCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            var bindingWorker = new NetTcpBinding();
            bindingWorker.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string addressWorker = "net.tcp://localhost:9998/LBDuplex";
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

            try
            {
                hostServer.Open();
                hostWorker.Open();
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

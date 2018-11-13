using CertificateManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            const string serviceCertCN = "wcfservice";

            var binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            var serviceCertificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, serviceCertCN);
            
            string address = "net.tcp://localhost:9998/LBDuplex";
            var endpoint = new EndpointAddress(new Uri(address), new X509CertificateEndpointIdentity(serviceCertificate));
            var handler = new CallBackHandler();

            using (var proxy = new LBDuplexClient(handler, binding, address))
            {
                proxy.SignIn(100);
                Console.WriteLine("Signing in...");
                while (true) ;
            }
        }
    }
}

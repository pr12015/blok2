using CertificateManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Timers;

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
            
            var address = "net.tcp://localhost:9998/LBDuplexWorker";
            var endpoint = new EndpointAddress(new Uri(address), new X509CertificateEndpointIdentity(serviceCertificate));
            var handler = new CallBackHandler();

            using (var proxy = new LBDuplexClient(handler, binding, endpoint))
            {
                proxy.Subscribe();
                Console.WriteLine("Subscribed to LoadBalancer.");

                var timer = new Timer
                {
                    /// timer interval is extracted from command line argument.
                    /// Interval = double.Parse(args[1])
                    Interval = 10000
                };

                timer.Elapsed += delegate { OnTimedEvent(proxy); };
                //timer.Start();

                Console.ReadKey(false);
            }
        }

        private static void OnTimedEvent(LBDuplexClient proxy)
        {
            proxy.UnSubscribe();
            Environment.Exit(0);
        }
    }
}

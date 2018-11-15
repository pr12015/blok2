using System;
using System.ServiceModel;
using Contracts;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;
using CertificateManager;
using System.Security.Principal;
using EventLogger;

namespace Worker
{
    public class LBDuplexClient : DuplexChannelFactory<IWorker>, IDisposable
    {
        IWorker proxy;
        public Logger logger;

        public LBDuplexClient(CallBackHandler handler, NetTcpBinding binding, EndpointAddress address)
            : base(new InstanceContext(handler), binding, address)
        {
            //logger = new Logger("LB.Audit", "LoadBalancerLog");
            string clientCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, clientCertCN);

            proxy = this.CreateChannel();
        }

        public void Dispose()
        {
            if (proxy != null)
                proxy = null;

            this.Close();
        }

        public void Subscribe()
        {
            try
            {
                proxy.Subscribe();
                //logger.Log("wcfclient", EventLogger.EventType.AuthenticationSuccess);
            }
            catch(Exception e)
            {
                //logger.Log("wcfclient", EventLogger.EventType.AuthenticationFailure);
            }
        }

        public void UnSubscribe()
        {
            proxy.UnSubscribe();
        }
    }
}

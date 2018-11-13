using System;
using System.ServiceModel;
using Contracts;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;
using CertificateManager;
using System.Security.Principal;

namespace Worker
{
    public class LBDuplexClient : DuplexChannelFactory<IWorker>, IDisposable
    {
        IWorker proxy;

        public LBDuplexClient(CallBackHandler handler, NetTcpBinding binding, EndpointAddress address)
            : base(new InstanceContext(handler), binding, address)
        {
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
            proxy.Subscribe();
        }

        public void UnSubscribe()
        {
            proxy.UnSubscribe();
        }
    }
}

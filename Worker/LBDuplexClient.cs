using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;

namespace Worker
{
    public class LBDuplexClient : DuplexChannelFactory<IWorker>, IDisposable
    {
        IWorker proxy;

        public LBDuplexClient(CallBackHandler handler, NetTcpBinding binding, string address)
            : base(new InstanceContext(handler), binding, address)
        {
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

using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class LoadBalancerClient : ChannelFactory<IServer>, IServer, IDisposable    
    {
        IServer factory;

        public LoadBalancerClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public double RequestBill(double value)
        {
            try
            {
                return factory.RequestBill(value);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        public void Dispose()
        {
            if (factory != null)
                factory = null;

            this.Close();
        }
    }
}

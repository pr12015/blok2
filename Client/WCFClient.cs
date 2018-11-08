using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;

namespace Client
{
    class WCFClient : ChannelFactory<ISmartMeterService>, ISmartMeterService, IDisposable
    {
        ISmartMeterService factory;

        public WCFClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = this.CreateChannel();
        }


        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public bool ModifyDB(int newValue)
        {
            bool allowed = false;

            try
            {
                allowed = factory.ModifyDB(newValue);
                Console.WriteLine("Modify() : {0}", allowed);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Modify(). {0}", e.Message);
            }

            return allowed;
        }

        public bool AddDB()
        {
            bool allowed = false;
            try
            {
                allowed = factory.AddDB();
                Console.WriteLine("Add() : {0}", allowed);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to AddDB(). {0}", e.Message);
            }

            return allowed;
        }

        public bool DeleteEntityDB()
        {
            bool allowed = false;
            try
            {
                allowed = factory.DeleteEntityDB();
                Console.WriteLine("DeleteEntityDB() : {0}", allowed);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to DeleteEntityDB(). {0}", e.Message);
            }

            return allowed;
        }

        public bool DeleteDB()
        {
            bool allowed = false;
            try
            {
                allowed = factory.DeleteDB();
                Console.WriteLine("DeleteDB() : {0}", allowed);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to DeleteDB(). {0}", e.Message);
            }

            return allowed;
        }

        public int GetBill()
        {
            int value = -1;

            try
            {
                value = factory.GetBill();
                Console.WriteLine("Read() Value: {0}", value);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Read(). {0}", e.Message);
            }

            return value;
        }
    }
}

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

        public bool ModifyID(int newValue, int oldValue)
        {
            bool allowed = false;

            try
            {
                allowed = factory.ModifyID(newValue, oldValue);
                Console.WriteLine("Modify() : {0}", allowed);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Modify(). {0}", e.Message);
            }

            return allowed;
        }

        public bool ModifyReading(double newValue, int id)
        {
            bool modified = false;

            try
            {
                modified = factory.ModifyReading(newValue, id);
                Console.WriteLine("Modify() : {0}", modified);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Modify(). {0}", e.Message);
            }

            return modified;
        }

        public bool AddDB(int id, string fullName, double reading)
        {
            bool allowed = false;
            try
            {
                allowed = factory.AddDB(id, fullName, reading);
                Console.WriteLine("Add() : {0}", allowed);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to AddDB(). {0}", e.Message);
            }

            return allowed;
        }

        public bool DeleteEntityDB(int id)
        {
            bool allowed = false;
            try
            {
                allowed = factory.DeleteEntityDB(id);
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

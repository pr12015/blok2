using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel; 

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/WCFService";

            using (WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(address))))
            {

                proxy.GetBill(129);
                //proxy.AddDB(121, "Petar", 923.12);
                //proxy.AddDB(122, "Petar", 2123.12);
                //proxy.AddDB(123, "Petar", 523.12);
                //proxy.AddDB(124, "Petar", 1233.12);
                //proxy.AddDB(125, "Petar", 1232.12);
                //proxy.AddDB(126, "Petar", 1231.12);
                //proxy.AddDB(127, "Petar", 1123.12);
                //proxy.AddDB(128, "Petar", 13.12);
                //proxy.AddDB(129, "Petar", 23.2);
                //proxy.AddDB(131, "Petar", 123.12);
                //proxy.AddDB(143, "Petar", 12.12);
                //proxy.AddDB(153, "Petar", 23.1);
                //proxy.AddDB(163, "Petar", 423.12);
                //proxy.ModifyID(321, 123);
                //proxy.ModifyReading(431.12, 321);
                //proxy.DeleteEntityDB(123);
                //proxy.DeleteDB();
                //proxy.GetBill(123);
            }

            Console.ReadLine();
        }
    }
}

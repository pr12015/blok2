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
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

            string address = "net.tcp://localhost:9999/WCFService";

            var rand = new Random();
            using (WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(address))))
            {
                /// If client was started via ClientTest.
                if (args.Length > 0)
                {
                    while (true)
                    {
                        System.Threading.Thread.Sleep(rand.Next(500, 1000));
                        proxy.GetBill(rand.Next(10000000, 10000017));
                    }
                }   
                /// If client was started normally.
                else
                {
                    //for(int i = 0; i < 100; ++i)
                    //{
                    //    proxy.AddDB(10000000 + i, "pera peric", rand.NextDouble() * rand.Next(100, 10000));
                    //}                    

                    Menu(proxy);
                }
            }

            Console.ReadLine();
        }

        // TODO:
        public static void Menu(WCFClient proxy)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. GetBill.");
                Console.WriteLine("2. ModifyID.");
                Console.WriteLine("3. ModifyReading.");
                Console.WriteLine("4. DeleteEntity.");
                Console.WriteLine("5. DeleteDB.");
                Console.WriteLine("6. AddDB.");
                Console.WriteLine("-----------------------");
                Console.Write("Enter your choice: ");

                int choice = 0;
                if (!int.TryParse(Console.ReadLine(), out choice))
                    continue;
                int id = 0, newID = 0;
                double newReading = 0;

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter ID: ");
                        if(int.TryParse(Console.ReadLine(), out id))
                            proxy.GetBill(id);
                        break;
                    case 2:
                        Console.Write("Enter ID: ");
                        if (int.TryParse(Console.ReadLine(), out id))
                        {
                            Console.Write("Enter new id: ");
                            if (int.TryParse(Console.ReadLine(), out newID))
                                proxy.ModifyID(newID, id);
                        }
                        break;
                    case 3:
                        Console.Write("Enter ID: ");
                        if (int.TryParse(Console.ReadLine(), out id))
                        {
                            Console.Write("Enter new reading: ");
                            if (double.TryParse(Console.ReadLine(), out newReading))
                                proxy.ModifyReading(newReading, id);
                        }
                        break;
                    case 4:
                        Console.Write("Enter ID: ");
                        if(int.TryParse(Console.ReadLine(), out id))
                            proxy.DeleteEntityDB(id);
                        break;
                    case 5:
                        proxy.DeleteDB();
                        break;
                    case 6:
                        Console.Write("Enter ID: ");
                        if (int.TryParse(Console.ReadLine(), out id))
                        {
                            Console.Write("Enter full name: ");
                            var name = Console.ReadLine();
                            Console.Write("Enter reading: ");
                            if (double.TryParse(Console.ReadLine(), out newReading))
                                proxy.AddDB(id, name, newReading);
                        }
                        break;
                    default:
                        break;
                }
                Console.Write("Press <enter> continue...");
                Console.ReadLine();
            }
        }
    }
}

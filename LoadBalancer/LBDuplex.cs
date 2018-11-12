using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;

namespace LoadBalancer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class LBDuplex : IWorker, IServer
    {
        static int cost = 0;
        public static List<Tuple<IWorkerCallBack, int>> workersFree = new List<Tuple<IWorkerCallBack, int>>();
        public static List<Tuple<IWorkerCallBack, int>> workersBusy = new List<Tuple<IWorkerCallBack, int>>();

        //public static Tuple<int, List<IWorkerCallBack>> pair = new Tuple<int, List<IWorkerCallBack>>(cost, workersFree);
        public void SignIn(int value)
        {
            Console.WriteLine("Client Signed in with value: " + value);
            workersFree.Add(new Tuple<IWorkerCallBack, int>(CallBack, ++cost));
        }

        public void SignOut(int value)
        {
            Console.WriteLine("Client Signed out with value: " + value);
            int i = 1;
            foreach (var callBack in workersFree)
            {
                if (callBack.Item1 == CallBack)
                {
                    workersFree.RemoveAt(i);
                }
                i++;
            }
        }

        public static double GetBill(double value)
        {
            //var callBack = workersFree[0];
            workersBusy.Add(workersFree[0]);
            workersFree.RemoveAt(0);
            //Console.WriteLine("Client sent: " + workersBusy[0].GetBill(value));
            return workersBusy[0].Item1.GetBill(value);
        }

        public double RequestBill(double value)
        {
            return GetBill(value);
        }

        static IWorkerCallBack CallBack
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IWorkerCallBack>();
            }
        }
    }
}

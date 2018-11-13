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
    [CallbackBehavior(IncludeExceptionDetailInFaults = true)]
    public class LBDuplex : IWorker, IServer
    {
        private static readonly object _workerLock = new object();
        //private static readonly object removeLock = new object();

        static int cost = 0;
        public static List<Pair<IWorkerCallBack, int>> workersFree = new List<Pair<IWorkerCallBack, int>>();
        // public static List<Pair<IWorkerCallBack, int>> workersBusy = new List<Pair<IWorkerCallBack, int>>();

        public void Subscribe()
        {
            Console.WriteLine("Worker subscribed.");

            lock (_workerLock)
            {
                workersFree.Add(new Pair<IWorkerCallBack, int>(CallBack, ++cost));
            }
        }

        public void UnSubscribe()
        {
            Console.WriteLine("Worket unsubscribed.");

            int i = 0;
            bool reCalculate = false;
            lock (_workerLock)
            {
                foreach (var callBack in workersFree)
                {
                    /// Unsubscribe worker.
                    if (callBack.First == CallBack)
                    {
                        workersFree.RemoveAt(i);
                        reCalculate = true;
                        break;
                    }
                    ++i;
                }

                /// Recalculate the cost factor of the rest of the workers.
                if (reCalculate)
                {
                    for (; i < workersFree.Count; ++i)
                        workersFree[i].Second -= 1;
                }
            }
        }

        private Pair<IWorkerCallBack, int> GetFreeWorker()
        {
            var worker = workersFree[0];
            lock (_workerLock)
            {
                workersFree.RemoveAt(0);
            }

            return worker;
        }

        private void InsertWorker(Pair<IWorkerCallBack, int> worker)
        {
            lock (_workerLock)
            {
                if (worker.Second > workersFree.Count + 1)
                    workersFree.Insert(worker.Second - 1, worker);
                else
                    workersFree.Add(worker);
            }
        }

        private double GetBill(double value)
        {
            var worker = GetFreeWorker();
            double result;

            /// Try until free worker is found and bill calculated.
            while (true)
            {
                try
                {
                    result = worker.First.GetBill(value);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    worker = GetFreeWorker();
                }
            }

            InsertWorker(worker);

            return result;
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

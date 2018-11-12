using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker
{
    public class CallBackHandler : Contracts.IWorkerCallBack
    {
        public double GetBill(double value)
        {
            return --value;
        }
    }
}

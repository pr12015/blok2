﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IWorkerCallBack))]
    public interface IWorker
    {
        [OperationContract]
        void Subscribe();

        [OperationContract]
        void UnSubscribe();
    }
}

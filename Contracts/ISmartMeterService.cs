using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface ISmartMeterService
    {
        [OperationContract]
        bool ModifyDB(int newValue);

        [OperationContract]
        bool AddDB();

        [OperationContract]
        bool DeleteEntityDB();

        [OperationContract]
        bool DeleteDB();

        [OperationContract]
        int GetBill();
    }
}

using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface ISmartMeterService
    {
        [OperationContract]
        bool ModifyID(int newValue, int oldValue);

        [OperationContract]
        bool ModifyReading(double newValue, int id);

        [OperationContract]
        bool AddDB(int id, string fullName, double reading);

        [OperationContract]
        bool DeleteEntityDB(int id);

        [OperationContract]
        bool DeleteDB();

        [OperationContract]
        int GetBill();
    }
}

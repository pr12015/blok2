using System.ServiceModel;

namespace Contracts
{
    [ServiceContract]
    public interface ISmartMeterService
    {
        /// <summary>
        /// Modifies the ID of database entry.
        /// Only operators can perform this action.
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        [OperationContract]
        bool ModifyID(int newValue, int oldValue);

        /// <summary>
        /// Modifies the Reading of database entry.
        /// Only operators can perform this action.
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="id"></param>
        [OperationContract]
        bool ModifyReading(double newValue, int id);

        /// <summary>
        /// Adds new entry to the database.
        /// Only admins can perform this action.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fullName"></param>
        /// <param name="reading"></param>
        [OperationContract]
        bool AddDB(int id, string fullName, double reading);

        /// <summary>
        /// Deletes entity, if exists, from the database.
        /// Only admins can perform this action.
        /// </summary>
        /// <param name="id"></param>
        [OperationContract]
        bool DeleteEntityDB(int id);

        /// <summary>
        /// Deletes entire database.
        /// Only SuperUsers are allowed to perform this action.
        /// </summary>
        [OperationContract]
        bool DeleteDB();

        /// <summary>
        /// Gets the current bill for the EMeter with id ID.
        /// Every one is allowed to perform this action.
        /// </summary>
        /// <param name="id"></param>
        [OperationContract]
        double GetBill(int id);
    }
}

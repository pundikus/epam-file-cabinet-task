using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// This Interface describes the service.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// This Method flush marks record.
        /// </summary>
        /// <returns>Count removed records.</returns>
        int Purge();

        /// <summary>
        /// This Method remove record.
        /// </summary>
        /// <param name="id">Id record.</param>
        /// <returns>Id removed record.</returns>
        int RemoveRecord(int id);

        /// <summary>
        /// This Method make snapshot collection.
        /// </summary>
        /// <returns>snapshot collection.</returns>
        FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// This method is for creating records.
        /// </summary>
        /// <param name="parametrs">It is object parametrs.</param>
        /// <returns>unique identifier for user.</returns>
        int CreateRecord(FileCabinetRecord parametrs);

        /// <summary>
        /// This method is for changes records.
        /// </summary>
        /// <param name="parametrs">It is object parametrs.</param>
        /// <returns>id edit record.</returns>
        int EditRecord(FileCabinetRecord parametrs);

        /// <summary>
        /// This method load records from file.
        /// </summary>
        /// <param name="snapshot">It is copy data.</param>
        /// <returns>Return record.</returns>
        int Restore(FileCabinetServiceSnapshot snapshot);

        /// <summary>
        /// Searches the records by id.
        /// </summary>
        /// <param name="id">Given id.</param>
        /// <returns>The array of records.</returns>
        IEnumerable<FileCabinetRecord> FindById(string id);

        /// <summary>
        /// This Method find records by Date of Birth users.
        /// </summary>
        /// <param name="dateOfBirth">Date of Birth users.</param>
        /// <returns>an array with entries by Date of Birth.</returns>
        IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        /// <summary>
        /// This Method find records by Firstname users.
        /// </summary>
        /// <param name="firstName">Firstname users.</param>
        /// <returns>an array with entries by Firstname.</returns>
        IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// This Method find records by Lastname users.
        /// </summary>
        /// <param name="lastName">Lastname users.</param>
        /// <returns>an array with entries by Lastname.</returns>
        IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Searches the records by Cabinet number.
        /// </summary>
        /// <param name="cabinetNumber">Given Cabinet number.</param>
        /// <returns>The array of records.</returns>
        IEnumerable<FileCabinetRecord> FindByCabinetNumber(string cabinetNumber);

        /// <summary>
        /// Searches the records by Category.
        /// </summary>
        /// <param name="category">Given Category.</param>
        /// <returns>The array of records.</returns>
        IEnumerable<FileCabinetRecord> FindByCategory(string category);

        /// <summary>
        /// Searches the records by Salary.
        /// </summary>
        /// <param name="salary">Given Salary.</param>
        /// <returns>The array of records.</returns>
        IEnumerable<FileCabinetRecord> FindBySalary(string salary);

        /// <summary>
        /// This Method gets all records as an array.
        /// </summary>
        /// <returns>array all records.</returns>
        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Returns the number of deleted records in the list.
        /// </summary>
        /// <returns>The number of deleted records.</returns>
        int GetDeleted();

        /// <summary>
        /// This Method returns count records.
        /// </summary>
        /// <returns>count records.</returns>
        int GetStat();

        /// <summary>
        /// This Method returns record.
        /// </summary>
        /// <param name="record">record.</param>
        /// <returns>Record.</returns>
        int AddRecord(FileCabinetRecord record);
    }
}
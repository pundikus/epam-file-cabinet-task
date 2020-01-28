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
        public int PurgeRecords();

        /// <summary>
        /// This Method remove record.
        /// </summary>
        /// <param name="id">Id record.</param>
        /// <returns>Id removed record.</returns>
        public int RemoveRecord(int id);

        /// <summary>
        /// This Method make snapshot collection.
        /// </summary>
        /// <returns>snapshot collection.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// This method is for creating records.
        /// </summary>
        /// <param name="parametrs">It is object parametrs.</param>
        /// <returns>unique identifier for user.</returns>
        public int CreateRecord(FileCabinetRecord parametrs);

        /// <summary>
        /// This method is for changes records.
        /// </summary>
        /// <param name="parametrs">It is object parametrs.</param>
        /// <returns>id edit record.</returns>
        public int EditRecord(FileCabinetRecord parametrs);

        /// <summary>
        /// This method load records from file.
        /// </summary>
        /// <param name="snapshot">It is copy data.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot);

        /// <summary>
        /// This Method find records by Date of Birth users.
        /// </summary>
        /// <param name="dateOfBirth">Date of Birth users.</param>
        /// <returns>an array with entries by Date of Birth.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        /// <summary>
        /// This Method find records by Firstname users.
        /// </summary>
        /// <param name="firstName">Firstname users.</param>
        /// <returns>an array with entries by Firstname.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// This Method find records by Lastname users.
        /// </summary>
        /// <param name="lastName">Lastname users.</param>
        /// <returns>an array with entries by Lastname.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// This Method gets all records as an array.
        /// </summary>
        /// <returns>array all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// This Method returns count records.
        /// </summary>
        /// <returns>count records.</returns>
        public int GetStat();
    }
}
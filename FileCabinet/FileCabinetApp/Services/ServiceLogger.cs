using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for adding loggs to the file.
    /// </summary>
    public class ServiceLogger : IFileCabinetService
    {
        private IFileCabinetService service;

        private StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="service">Service to decorate.</param>
        /// <param name="writer">Writer to write to.</param>
        public ServiceLogger(IFileCabinetService service, StreamWriter writer)
        {
            this.service = service;
            this.writer = writer;
        }

        /// <summary>
        /// Adds logs about AddRecord method in the textfile.
        /// </summary>
        /// <param name="record">Record to add.</param>
        /// <returns>Record's id.</returns>
        public int AddRecord(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException($"Record is invalid.");
            }

            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling Add() with FirstName = '" + record.FirstName + "', LastName = '" + record.LastName + "', DateOfBirth = '" + record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "'\n";
            this.writer.Write(toWrite);
            int id = this.service.AddRecord(record);
            toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + "Add() returned " + id + "\n";
            this.writer.Write(toWrite);
            return id;
        }

        /// <summary>
        /// Adds logs about CreateRecord method in the textfile.
        /// </summary>
        /// <param name="record">Record to create.</param>
        /// <returns>Record's id.</returns>
        public int CreateRecord(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException($"Record is invalid.");
            }

            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling Create() with FirstName = '" + record.FirstName + "', LastName = '" + record.LastName + "', DateOfBirth = '" + record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "'" + "\n";
            this.writer.Write(toWrite);

            int id = this.service.CreateRecord(record);

            toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Create() returned " + id + "\n";
            this.writer.Write(toWrite);

            return id;
        }

        /// <summary>
        /// Adds logs about EditRecord method in the textfile.
        /// </summary>
        /// <param name="record">Record to edit.</param>
        /// <returns>Whether operation succeeded.</returns>
        public int EditRecord(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException($"Record is invalid.");
            }

            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling Edit() with FirstName = '" + record.FirstName + "', LastName = '" + record.LastName + "', DateOfBirth = '" + record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "'" + "\n";
            this.writer.Write(toWrite);

            int result = this.service.EditRecord(record);

            toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Edit() returned " + result + "\n";
            this.writer.Write(toWrite);

            return result;
        }

        /// <summary>
        /// Adds logs about Id in the textfile.
        /// </summary>
        /// <param name="id">Id to find.</param>
        /// <returns>List of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindById(string id)
        {
            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling Find() with FirstName'" + id + "'" + "\n";
            this.writer.Write(toWrite);

            var records = this.service.FindById(id);

            if (records != null)
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() returned records with ids ";

                foreach (var record in records)
                {
                    toWrite += record.Id;
                    toWrite += " ";
                }

                toWrite += "\n";
            }
            else
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() found no records.";
            }

            toWrite += "\n";

            this.writer.Write(toWrite);

            return records;
        }

        /// <summary>
        /// Adds logs about FindByFirstName method in the textfile.
        /// </summary>
        /// <param name="firstName">First name to find by.</param>
        /// <returns>List of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling Find() with FirstName'" + firstName + "'" + "\n";

            this.writer.Write(toWrite);

            IEnumerable<FileCabinetRecord> records = this.service.FindByFirstName(firstName);

            if (records != null)
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() returned records with ids ";

                foreach (var record in records)
                {
                    toWrite += record.Id;
                    toWrite += " ";
                }

                toWrite += "\n";
            }
            else
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() found no records.";
            }

            toWrite += "\n";

            this.writer.Write(toWrite);

            return records;
        }

        /// <summary>
        /// Adds logs about FindByLastName method in the textfile.
        /// </summary>
        /// <param name="lastName">Last name to find by.</param>
        /// <returns>List of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling Find() with LastName'" + lastName + "'" + "\n";
            this.writer.Write(toWrite);

            IEnumerable<FileCabinetRecord> records = this.service.FindByLastName(lastName);

            if (records != null || records.ToList().Count != 0)
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() returned records with ids ";

                foreach (var record in records)
                {
                    toWrite += record.Id;
                    toWrite += " ";
                }

                toWrite += "\n";
            }
            else
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() found no records.";
            }

            toWrite += "\n";

            this.writer.Write(toWrite);

            return records;
        }

        /// <summary>
        /// Adds logs about FindByDateOfBirth method in the textfile.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth to find by.</param>
        /// <returns>List of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling Find() with DateOfBirth'" + dateOfBirth + "'" + "\n";
            this.writer.Write(toWrite);

            var records = this.service.FindByDateOfBirth(dateOfBirth);

            if (records != null)
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() returned records with ids ";

                foreach (var record in records)
                {
                    toWrite += record.Id;
                    toWrite += " ";
                }

                toWrite += "\n";
            }
            else
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() found no records.";
            }

            toWrite += "\n";

            this.writer.Write(toWrite);

            return records;
        }

        /// <summary>
        /// Adds logs about FindByCabinetNumber in the textfile.
        /// </summary>
        /// <param name="cabinetNumber">Cabinet Number to find.</param>
        /// <returns>List of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByCabinetNumber(string cabinetNumber)
        {
            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling Find() with DateOfBirth'" + cabinetNumber + "'" + "\n";
            this.writer.Write(toWrite);

            var records = this.service.FindByCabinetNumber(cabinetNumber);

            if (records != null)
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() returned records with ids ";

                foreach (var record in records)
                {
                    toWrite += record.Id;
                    toWrite += " ";
                }

                toWrite += "\n";
            }
            else
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() found no records.";
            }

            toWrite += "\n";

            this.writer.Write(toWrite);

            return records;
        }

        /// <summary>
        /// Adds logs about Category in the textfile.
        /// </summary>
        /// <param name="category">Category to find.</param>
        /// <returns>List of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByCategory(string category)
        {
            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling Find() with DateOfBirth'" + category + "'" + "\n";
            this.writer.Write(toWrite);

            var records = this.service.FindByCategory(category);

            if (records != null)
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() returned records with ids ";

                foreach (var record in records)
                {
                    toWrite += record.Id;
                    toWrite += " ";
                }

                toWrite += "\n";
            }
            else
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() found no records.";
            }

            toWrite += "\n";

            this.writer.Write(toWrite);

            return records;
        }

        /// <summary>
        /// Adds logs about FindBySalary method in the textfile.
        /// </summary>
        /// <param name="salary">Salary to find.</param>
        /// <returns>List of found records.</returns>
        public IEnumerable<FileCabinetRecord> FindBySalary(string salary)
        {
            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling Find() with DateOfBirth'" + salary + "'" + "\n";
            this.writer.Write(toWrite);

            var records = this.service.FindBySalary(salary);

            if (records != null)
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() returned records with ids ";

                foreach (var record in records)
                {
                    toWrite += record.Id;
                    toWrite += " ";
                }

                toWrite += "\n";
            }
            else
            {
                toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Find() found no records.";
            }

            toWrite += "\n";

            this.writer.Write(toWrite);

            return records;
        }

        /// <summary>
        /// Adds logs about GetRecords method in the textfile.
        /// </summary>
        /// <returns>The list of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling GetRecords()" + "\n";
            this.writer.Write(toWrite);

            ReadOnlyCollection<FileCabinetRecord> records = this.service.GetRecords();

            toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " GetRecords() returned " + records.Count + " records." + "\n";
            this.writer.Write(toWrite);

            return records;
        }

        /// <summary>
        /// Adds logs about GetStat method in the textfile.
        /// </summary>
        /// <returns>The number of records.</returns>
        public int GetStat()
        {
            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling GetStat()" + "\n";
            this.writer.Write(toWrite);

            int recordsCount = this.service.GetStat();

            toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " GetStat() returned " + recordsCount + "\n";
            this.writer.Write(toWrite);

            return recordsCount;
        }

        /// <summary>
        /// Adds logs about MakeSnapshot method in the textfile.
        /// </summary>
        /// <returns>An instance of the IFileCabinetServiceSnapshot class.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling MakeSnapshot()" + "\n";
            this.writer.Write(toWrite);

            var snapshot = this.service.MakeSnapshot();

            toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " MakeSnapshot() returned the snapshot of records." + "\n";
            this.writer.Write(toWrite);

            return snapshot;
        }

        /// <summary>
        /// Add logs about Purge method in the textfile.
        /// </summary>
        /// <returns>count removed records.</returns>
        public int Purge()
        {
            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling Remove()\n";
            this.writer.Write(toWrite);

            int count = this.service.Purge();

            toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Remove() removed " + count + "records \n";
            this.writer.Write(toWrite);

            return count;
        }

        /// <summary>
        /// Adds logs about Remove method in the textfile.
        /// </summary>
        /// <param name="id">Id to remove by.</param>
        /// <returns>Id of removed record.</returns>
        public int RemoveRecord(int id)
        {
            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling Remove()\n";
            this.writer.Write(toWrite);

            int removedId = this.service.RemoveRecord(id);

            toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Remove() removed record with id " + removedId + "\n";
            this.writer.Write(toWrite);

            return removedId;
        }

        /// <summary>
        /// Adds logs about Restore method in the textfile.
        /// </summary>
        /// <param name="snapshot">Snapshot to restore by.</param>
        /// <returns>count records.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            string toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Calling Restore()\n";
            this.writer.Write(toWrite);

            this.service.Restore(snapshot);

            toWrite = DateTime.Now.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture) + " Restore() imported " + snapshot.Records.Count + "records\n";
            this.writer.Write(toWrite);

            return snapshot.Records.Count;
        }

        /// <inheritdoc/>
        public int GetDeleted()
        {
            throw new NotImplementedException();
        }
    }
}

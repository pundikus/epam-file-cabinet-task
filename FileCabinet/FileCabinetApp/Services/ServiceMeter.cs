using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for measuring timings of service's methods.
    /// </summary>
    public class ServiceMeter : IFileCabinetService
    {
        private IFileCabinetService service;

        private Stopwatch watch;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">Service for measuring.</param>
        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public int AddRecord(FileCabinetRecord record)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds timing count for create method.
        /// </summary>
        /// <param name="record">Record to create.</param>
        /// <returns>Record's id.</returns>
        public int CreateRecord(FileCabinetRecord record)
        {
            this.watch = Stopwatch.StartNew();
            int id = this.service.CreateRecord(record);
            this.watch.Stop();
            Console.WriteLine($"Create method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return id;
        }

        /// <summary>
        /// Adds timing count for edit method.
        /// </summary>
        /// <param name="record">Record to edit.</param>
        /// <returns>Whether operation succeeded.</returns>
        public int EditRecord(FileCabinetRecord record)
        {
            this.watch = Stopwatch.StartNew();
            int id = this.service.EditRecord(record);
            this.watch.Stop();
            Console.WriteLine($"Edit method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return id;
        }

        /// <summary>
        /// Adds timing count for Id.
        /// </summary>
        /// <param name="id">Given Id.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindById(string id)
        {
            this.watch = Stopwatch.StartNew();
            var foundRecords = this.service.FindById(id);
            this.watch.Stop();
            Console.WriteLine($"FindById method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return foundRecords;
        }

        /// <summary>
        /// Adds timing count for FindByFirstName method.
        /// </summary>
        /// <param name="firstName">Given first name.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.watch = Stopwatch.StartNew();
            IEnumerable<FileCabinetRecord> foundRecords = this.service.FindByFirstName(firstName);
            this.watch.Stop();
            Console.WriteLine($"FindByFirstName method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return foundRecords;
        }

        /// <summary>
        /// Adds timing count for FindByLastName method.
        /// </summary>
        /// <param name="lastName">Given last name.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.watch = Stopwatch.StartNew();
            IEnumerable<FileCabinetRecord> foundRecords = this.service.FindByLastName(lastName);
            this.watch.Stop();
            Console.WriteLine($"FindByLastName method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return foundRecords;
        }

        /// <summary>
        /// Adds timing count for FindByDateOfBirth method.
        /// </summary>
        /// <param name="dateOfBirth">Given date of birth.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            this.watch = Stopwatch.StartNew();
            var foundRecords = this.service.FindByDateOfBirth(dateOfBirth);
            this.watch.Stop();
            Console.WriteLine($"FindByDateOfBirth method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return foundRecords;
        }

        /// <summary>
        /// Adds timing count for FindByCabinetNumber method.
        /// </summary>
        /// <param name="cabinetNumber">Given Cabinet number.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByCabinetNumber(string cabinetNumber)
        {
            this.watch = Stopwatch.StartNew();
            var foundRecords = this.service.FindByCabinetNumber(cabinetNumber);
            this.watch.Stop();
            Console.WriteLine($"FindByFavouriteNumber method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return foundRecords;
        }

        /// <summary>
        /// Adds timing count for Category.
        /// </summary>
        /// <param name="category">Given Category.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByCategory(string category)
        {
            this.watch = Stopwatch.StartNew();
            var foundRecords = this.service.FindByCategory(category);
            this.watch.Stop();
            Console.WriteLine($"FindByFavouriteCharacter method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return foundRecords;
        }

        /// <summary>
        /// Adds timing count for FindBySalary.
        /// </summary>
        /// <param name="salary">Given Salary.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindBySalary(string salary)
        {
            this.watch = Stopwatch.StartNew();
            var foundRecords = this.service.FindBySalary(salary);
            this.watch.Stop();
            Console.WriteLine($"FindBySalary method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return foundRecords;
        }

        /// <summary>
        /// Adds timing count for GetRecords method.
        /// </summary>
        /// <returns>The array of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.watch = Stopwatch.StartNew();
            ReadOnlyCollection<FileCabinetRecord> records = this.service.GetRecords();
            this.watch.Stop();
            Console.WriteLine($"GetRecords method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return records;
        }

        /// <summary>
        /// Adds timing count for GetStat method.
        /// </summary>
        /// <returns>The number of records.</returns>
        public int GetStat()
        {
            this.watch = Stopwatch.StartNew();
            int count = this.service.GetStat();
            this.watch.Stop();
            Console.WriteLine($"GetStat method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return count;
        }

        /// <summary>
        /// Adds timing count for MakeSnapshot method.
        /// </summary>
        /// <returns>>An instance of the IFileCabinetServiceSnapshot class.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.watch = Stopwatch.StartNew();
            var snapshot = this.service.MakeSnapshot();
            this.watch.Stop();
            Console.WriteLine($"MakeSnapshot method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return snapshot;
        }

        /// <summary>
        /// Adds timing count for Purge method.
        /// </summary>
        /// <returns>count removed elements.</returns>
        public int Purge()
        {
            this.watch = Stopwatch.StartNew();
            int count = this.service.Purge();
            this.watch.Stop();
            Console.WriteLine($"RemoveRecord method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return count;
        }

        /// <summary>
        /// Adds timing count for RemoveRecord method.
        /// </summary>
        /// <param name="id">Id of record to remove.</param>
        /// <returns>Id of removed record if succeeded, -1 otherwise.</returns>
        public int RemoveRecord(int id)
        {
            this.watch = Stopwatch.StartNew();
            int removedId = this.service.RemoveRecord(id);
            this.watch.Stop();
            Console.WriteLine($"RemoveRecord method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return removedId;
        }

        /// <summary>
        /// Adds timing count for Restore method.
        /// </summary>
        /// <param name="snapshot">A snapshot of records to restore.</param>
        /// <returns>count records.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            this.watch = Stopwatch.StartNew();
            this.service.Restore(snapshot);
            this.watch.Stop();
            Console.WriteLine($"Restore method execution duration is " + this.watch.ElapsedTicks + " ticks.");

            return snapshot.Records.Count;
        }

        /// <inheritdoc/>
        public int GetDeleted()
        {
            throw new NotImplementedException();
        }
    }
}

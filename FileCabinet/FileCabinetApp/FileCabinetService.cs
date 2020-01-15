using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// This is intended for service. It has all the necessary methods for creating, modifying, deleting records.
    /// </summary>
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="validator">validator for parameters.</param>
        public FileCabinetService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// This method is for creating records.
        /// </summary>
        /// <param name="parametrs">It is object parametrs.</param>
        /// <returns>unique identifier for user.</returns>
        public int CreateRecord(ValueRange parametrs)
        {
            if (parametrs == null)
            {
                throw new ArgumentNullException(nameof(parametrs));
            }

            this.CreateValidator().ValidateParameters(parametrs);

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = parametrs.FirstName,
                LastName = parametrs.LastName,
                DateOfBirth = parametrs.DateOfBirth,
                CabinetNumber = parametrs.CabinetNumber,
                Salary = parametrs.Salary,
                Category = parametrs.Category,
            };

            this.list.Add(record);

            this.AddRecordInAllDictionary(record);

            return record.Id;
        }

        /// <summary>
        /// This Method gets all records as an array.
        /// </summary>
        /// <returns>array all records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            if (this.list == null)
            {
                Console.WriteLine("List records is Empty.");
            }

            return this.list.ToArray();
        }

        /// <summary>
        /// This Method returns count records.
        /// </summary>
        /// <returns>count records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// This method is for changes records.
        /// </summary>
        /// <param name="parametrs">It is object parametrs.</param>
        public void EditRecord(ValueRange parametrs)
        {
            if (parametrs == null)
            {
                throw new ArgumentNullException(nameof(parametrs));
            }

            this.CreateValidator().ValidateParameters(parametrs);

            var record = new FileCabinetRecord
            {
                Id = parametrs.Id,
                FirstName = parametrs.FirstName,
                LastName = parametrs.LastName,
                DateOfBirth = parametrs.DateOfBirth,
                CabinetNumber = parametrs.CabinetNumber,
                Salary = parametrs.Salary,
                Category = parametrs.Category,
            };

            var recordById = this.list.Find(x => x.Id == parametrs.Id);

            this.list.Remove(recordById);
            this.list.Add(record);

            var newrecordById = this.list.Find(x => x.Id == parametrs.Id);

            this.DeleteRecordFromAllDictionary(recordById);
            this.AddRecordInAllDictionary(newrecordById);
        }

        /// <summary>
        /// This Method find records by Firstname users.
        /// </summary>
        /// <param name="firstName">Firstname users.</param>
        /// <returns>an array with entries by Firstname.</returns>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException($"{nameof(firstName)} not correct.");
            }

            return this.firstNameDictionary[firstName].ToArray();
        }

        /// <summary>
        /// This Method find records by Lastname users.
        /// </summary>
        /// <param name="lastName">Lastname users.</param>
        /// <returns>an array with entries by Lastname.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException($"{nameof(lastName)} not correct.");
            }

            return this.lastNameDictionary[lastName].ToArray();
        }

        /// <summary>
        /// This Method find records by Date of Birth users.
        /// </summary>
        /// <param name="dateOfBirth">Date of Birth users.</param>
        /// <returns>an array with entries by Date of Birth.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(string dateOfBirth)
        {
            if (dateOfBirth == null)
            {
                throw new ArgumentNullException(nameof(dateOfBirth));
            }

            string dateOfBirthString = dateOfBirth.ToString(CultureInfo.InvariantCulture);

            return this.dateOfBirthDictionary[dateOfBirthString].ToArray();
        }

        /// <summary>
        /// This Method create Validator.
        /// </summary>
        /// <returns>new Validator.</returns>
        protected virtual IRecordValidator CreateValidator()
        {
            return this.validator;
        }

        private static void AddRecord(Dictionary<string, List<FileCabinetRecord>> dictionary, string key, FileCabinetRecord record)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(record);
            }
            else
            {
                var newListRecords = new List<FileCabinetRecord>
                {
                    record,
                };

                dictionary.Add(key, newListRecords);
            }
        }

        private static void DeleteRecord(Dictionary<string, List<FileCabinetRecord>> dictionary, string key, FileCabinetRecord record)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Remove(record);
                if (dictionary[key].Count == 0)
                {
                    dictionary.Remove(key);
                }
            }
        }

        private static void AddRecordInFirstNameDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, FileCabinetRecord record)
        {
            var key = record.FirstName.ToUpperInvariant();
            AddRecord(dictionary, key, record);
        }

        private static void AddRecordInLastNameDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, FileCabinetRecord record)
        {
            var key = record.LastName.ToUpperInvariant();
            AddRecord(dictionary, key, record);
        }

        private static void AddRecordInDateofBirthDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, FileCabinetRecord record)
        {
            var key = record.DateOfBirth.ToString(CultureInfo.InvariantCulture).ToUpperInvariant();
            AddRecord(dictionary, key, record);
        }

        private static void DeleteRecordFromFirstNameDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, FileCabinetRecord record)
        {
            var key = record.FirstName.ToUpperInvariant();
            DeleteRecord(dictionary, key, record);
        }

        private static void DeleteRecordFromLastNameDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, FileCabinetRecord record)
        {
            var key = record.LastName.ToUpperInvariant();
            DeleteRecord(dictionary, key, record);
        }

        private static void DeleteRecordFromDateofBirthDictionary(Dictionary<string, List<FileCabinetRecord>> dictionary, FileCabinetRecord record)
        {
            var key = record.DateOfBirth.ToString(CultureInfo.InvariantCulture).ToUpperInvariant();
            DeleteRecord(dictionary, key, record);
        }

        private void DeleteRecordFromAllDictionary(FileCabinetRecord record)
        {
            DeleteRecordFromFirstNameDictionary(this.firstNameDictionary, record);
            DeleteRecordFromLastNameDictionary(this.lastNameDictionary, record);
            DeleteRecordFromDateofBirthDictionary(this.dateOfBirthDictionary, record);
        }

        private void AddRecordInAllDictionary(FileCabinetRecord record)
        {
            AddRecordInFirstNameDictionary(this.firstNameDictionary, record);
            AddRecordInLastNameDictionary(this.lastNameDictionary, record);
            AddRecordInDateofBirthDictionary(this.dateOfBirthDictionary, record);
        }
    }
}

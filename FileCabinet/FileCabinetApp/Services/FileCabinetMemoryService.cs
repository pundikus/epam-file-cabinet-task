using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using FileCabinetApp.Interfaces.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// This is intended for service. It has all the necessary methods for creating, modifying, deleting records.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private const int Deleted = 0;

        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private Dictionary<string, List<FileCabinetRecord>> idvDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private Dictionary<string, List<FileCabinetRecord>> cabinetNumberDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private Dictionary<string, List<FileCabinetRecord>> categoryDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private Dictionary<string, List<FileCabinetRecord>> salaryDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        private Dictionary<string, IEnumerable<FileCabinetRecord>> idvCache = new Dictionary<string, IEnumerable<FileCabinetRecord>>();
        private Dictionary<string, IEnumerable<FileCabinetRecord>> firstNameCache = new Dictionary<string, IEnumerable<FileCabinetRecord>>();
        private Dictionary<string, IEnumerable<FileCabinetRecord>> lastNameCache = new Dictionary<string, IEnumerable<FileCabinetRecord>>();
        private Dictionary<string, IEnumerable<FileCabinetRecord>> dateOfBirthCache = new Dictionary<string, IEnumerable<FileCabinetRecord>>();
        private Dictionary<string, IEnumerable<FileCabinetRecord>> cabinetNumberCache = new Dictionary<string, IEnumerable<FileCabinetRecord>>();
        private Dictionary<string, IEnumerable<FileCabinetRecord>> categoryCache = new Dictionary<string, IEnumerable<FileCabinetRecord>>();
        private Dictionary<string, IEnumerable<FileCabinetRecord>> salaryCache = new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        private IRecordValidator validator;
        private List<int> idlist = new List<int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">validator for parameters.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// This Method gets copies part of the service behavior.
        /// </summary>
        /// <returns>Object to class FileCabinetServiceSnapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            var snapshot = new FileCabinetServiceSnapshot(this.list);

            return snapshot;
        }

        /// <summary>
        /// This method is for creating records.
        /// </summary>
        /// <param name="parametrs">It is object parametrs.</param>
        /// <returns>unique identifier for user.</returns>
        public int CreateRecord(FileCabinetRecord parametrs)
        {
            if (parametrs == null)
            {
                throw new ArgumentNullException(nameof(parametrs));
            }

            this.validator.Validate(parametrs);

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
        /// Adds record to the list of records.
        /// </summary>
        /// <param name="record">Record to add.</param>
        /// <returns>Record's id.</returns>
        public int AddRecord(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException($"Record object is invalid.");
            }

            this.validator.Validate(record);

            if (this.idlist.Contains(record.Id))
            {
                int indexOfPrev = this.list.FindIndex(rec => rec.Id.Equals(record.Id));
                this.firstNameDictionary[this.list[indexOfPrev].FirstName.ToLower(CultureInfo.InvariantCulture)].Remove(this.list[indexOfPrev]);
                this.lastNameDictionary[this.list[indexOfPrev].LastName.ToLower(CultureInfo.InvariantCulture)].Remove(this.list[indexOfPrev]);
                this.dateOfBirthDictionary[this.list[indexOfPrev].DateOfBirth.ToString("yyyy-MMM-d", CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture)].Remove(this.list[indexOfPrev]);
                this.list[indexOfPrev] = record;
            }
            else
            {
                this.list.Add(record);
                this.idlist.Add(record.Id);
            }

            this.AddRecordInAllDictionary(record);
            this.PurgeCache(record);
            return record.Id;
        }

        /// <summary>
        /// Returns the number of deleted records in the list.
        /// </summary>
        /// <returns>The number of deleted records.</returns>
        public int GetDeleted()
        {
            return Deleted;
        }

        /// <summary>
        /// This Method gets all records as an array.
        /// </summary>
        /// <returns>array all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            ReadOnlyCollection<FileCabinetRecord> readOnlyList = new ReadOnlyCollection<FileCabinetRecord>(this.list);

            return readOnlyList;
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
        /// <returns>id record.</returns>
        public int EditRecord(FileCabinetRecord parametrs)
        {
            if (parametrs == null)
            {
                throw new ArgumentNullException(nameof(parametrs));
            }

            this.validator.Validate(parametrs);

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

            this.list.RemoveAt(recordById.Id - 1);
            this.list.Insert(recordById.Id - 1, record);

            var newrecordById = this.list.Find(x => x.Id == parametrs.Id);

            this.DeleteRecordFromAllDictionary(recordById);
            this.AddRecordInAllDictionary(newrecordById);

            return recordById.Id;
        }

        /// <summary>
        /// Searches the records by id.
        /// </summary>
        /// <param name="id">Given id.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindById(string id)
        {
            return FindInCache(this.idvDictionary, this.idvCache, id);
        }

        /// <summary>
        /// This Method find records by Firstname users.
        /// </summary>
        /// <param name="firstName">Firstname users.</param>
        /// <returns>an array with entries by Firstname.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return FindInCache(this.firstNameDictionary, this.firstNameCache, firstName);
        }

        /// <summary>
        /// This Method find records by Lastname users.
        /// </summary>
        /// <param name="lastName">Lastname users.</param>
        /// <returns>an array with entries by Lastname.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            return FindInCache(this.lastNameDictionary, this.lastNameCache, lastName);
        }

        /// <summary>
        /// This Method find records by Date of Birth users.
        /// </summary>
        /// <param name="dateOfBirth">Date of Birth users.</param>
        /// <returns>an array with entries by Date of Birth.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            return FindInCache(this.dateOfBirthDictionary, this.dateOfBirthCache, dateOfBirth);
        }

        /// <summary>
        /// Searches the records by cabinet number.
        /// </summary>
        /// <param name="cabinetNumber">Given cabinet number.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByCabinetNumber(string cabinetNumber)
        {
            return FindInCache(this.cabinetNumberDictionary, this.cabinetNumberCache, cabinetNumber);
        }

        /// <summary>
        /// Searches the records by category.
        /// </summary>
        /// <param name="category">Given category.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByCategory(string category)
        {
            return FindInCache(this.categoryDictionary, this.categoryCache, category);
        }

        /// <summary>
        /// Searches the records by salary.
        /// </summary>
        /// <param name="salary">Given salary.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindBySalary(string salary)
        {
            return FindInCache(this.salaryDictionary, this.salaryCache, salary);
        }

        /// <summary>
        /// This method load records from file.
        /// </summary>
        /// <param name="snapshot">It is copy data.</param>
        /// <returns>records count.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            var records = snapshot.Records;
            if (this.GetRecords().Count == 0)
            {
                foreach (var recordImport in records)
                {
                    try
                    {
                        this.validator.Validate(recordImport);

                        this.list.Add(recordImport);
                        this.AddRecordInAllDictionary(recordImport);
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(recordImport.Id + ex.Message);
                    }
                }
            }
            else
            {
                foreach (var recordImport in records)
                {
                    this.validator.Validate(recordImport);

                    var recordById = this.list.Find(x => x.Id == recordImport.Id);

                    if (recordById != null)
                    {
                        this.EditRecord(recordImport);
                    }
                    else
                    {
                        this.list.Add(recordImport);
                        this.AddRecordInAllDictionary(recordImport);
                    }
                }
            }

            return this.list.Count;
        }

        /// <summary>
        /// This Method remove record by Id.
        /// </summary>
        /// <param name="id">It is Id record.</param>
        /// <returns>Removed record.</returns>
        public int RemoveRecord(int id)
        {
            var recordById = this.list.Find(x => x.Id == id);
            if (recordById == null)
            {
                return 0;
            }

            this.list.Remove(recordById);
            this.DeleteRecordFromAllDictionary(recordById);

            return recordById.Id;
        }

        /// <inheritdoc/>
        public int Purge()
        {
            throw new NotImplementedException();
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

        private static IEnumerable<FileCabinetRecord> FindByKey(string key, Dictionary<string, List<FileCabinetRecord>> dictionary)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key to find by is invalid");
            }

            if (dictionary.ContainsKey(key.ToLower(CultureInfo.InvariantCulture)))
            {
                IEnumerable<FileCabinetRecord> foundRecords = dictionary[key.ToLower(CultureInfo.InvariantCulture)];
                return foundRecords;
            }
            else
            {
                throw new ArgumentException("No records found.");
            }
        }

        private static IEnumerable<FileCabinetRecord> FindInCache(Dictionary<string, List<FileCabinetRecord>> dictionary, Dictionary<string, IEnumerable<FileCabinetRecord>> cache, string key)
        {
            IEnumerable<FileCabinetRecord> foundRecords;

            if (cache.ContainsKey(key))
            {
                foundRecords = cache[key];
            }
            else
            {
                foundRecords = FindByKey(key, dictionary);
                cache.Add(key, foundRecords);
            }

            return foundRecords;
        }

        private static void PurgeCache(Dictionary<string, IEnumerable<FileCabinetRecord>> cache, string key)
        {
            if (cache.ContainsKey(key))
            {
                cache.Remove(key);
            }
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

        private void PurgeCache(FileCabinetRecord record)
        {
            PurgeCache(this.firstNameCache, record.FirstName);
            PurgeCache(this.lastNameCache, record.LastName);
            PurgeCache(this.dateOfBirthCache, record.DateOfBirth.ToString("yyyy-MMM-d", CultureInfo.InvariantCulture));
        }
    }
}

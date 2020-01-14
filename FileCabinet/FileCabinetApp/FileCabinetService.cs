﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short cabinetNumber, decimal salary, char category)
        {
            Validate(firstName, lastName, dateOfBirth, cabinetNumber, salary, category);

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                CabinetNumber = cabinetNumber,
                Salary = salary,
                Category = category,
            };

            this.list.Add(record);

            this.AddRecordInAllDictionary(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short cabinetNumber, decimal salary, char category)
        {
            Validate(firstName, lastName, dateOfBirth, cabinetNumber, salary, category);

            var record = new FileCabinetRecord
            {
                 Id = id,
                 FirstName = firstName,
                 LastName = lastName,
                 DateOfBirth = dateOfBirth,
                 CabinetNumber = cabinetNumber,
                 Salary = salary,
                 Category = category,
            };

            var recordById = this.list.Find(x => x.Id == id);

            this.list.Remove(recordById);
            this.list.Add(record);

            var newrecordById = this.list.Find(x => x.Id == id);

            this.DeleteRecordFromAllDictionary(recordById);
            this.AddRecordInAllDictionary(newrecordById);
        }

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

        public FileCabinetRecord[] FindByDateOfBirth(string dateOfBirth)
        {
            if (dateOfBirth == null)
            {
                throw new ArgumentNullException(nameof(dateOfBirth));
            }

            string dateOfBirthString = dateOfBirth.ToString(CultureInfo.InvariantCulture);

            return this.dateOfBirthDictionary[dateOfBirthString].ToArray();
        }

        private static void Validate(string firstName, string lastName, DateTime dateOfBirth, short cabinetNumber, decimal salary, char category)
        {
            if (firstName == null || string.IsNullOrEmpty(firstName.Trim()))
            {
                throw new ArgumentNullException($"{nameof(firstName)} cannot be empty.");
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException($"{nameof(firstName)} is not correct.");
            }

            if (lastName == null || string.IsNullOrEmpty(lastName.Trim()))
            {
                throw new ArgumentNullException($"{nameof(lastName)} cannot be empty.");
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException($"{nameof(lastName)} is not correct.");
            }

            DateTime minDate = new DateTime(1950, 1, 1);
            DateTime maxDate = DateTime.Now;

            if (dateOfBirth < minDate || dateOfBirth > maxDate)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} is not correct.");
            }

            if (cabinetNumber < 1 || cabinetNumber > 1500)
            {
                throw new ArgumentException($"{nameof(cabinetNumber)} is not correct.");
            }

            if (salary < 0 || salary > decimal.MaxValue)
            {
                throw new ArgumentException($"{nameof(salary)} is not correct.");
            }

            if (category < 65 || category > 67)
            {
                throw new ArgumentException($"{nameof(category)} is not correct.");
            }
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

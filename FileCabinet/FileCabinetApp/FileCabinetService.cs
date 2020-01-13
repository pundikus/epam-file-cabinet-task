using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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

            string firstNameUpper = firstName.ToUpperInvariant();
            string lastNameUpper = lastName.ToUpperInvariant();
            string dateOfBirthUpper = dateOfBirth.ToString(CultureInfo.InvariantCulture).ToUpperInvariant();

            if (this.firstNameDictionary.ContainsKey(firstNameUpper))
            {
                this.firstNameDictionary[firstNameUpper].Add(record);
            }
            else
            {
                var newList = new List<FileCabinetRecord>();
                newList.Add(record);
                this.firstNameDictionary.Add(firstNameUpper, newList);
            }

            if (this.lastNameDictionary.ContainsKey(lastNameUpper))
            {
                this.lastNameDictionary[lastNameUpper].Add(record);
            }
            else
            {
                var newList = new List<FileCabinetRecord>();
                newList.Add(record);
                this.lastNameDictionary.Add(lastNameUpper, newList);
            }

            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirthUpper))
            {
                this.dateOfBirthDictionary[dateOfBirthUpper].Add(record);
            }
            else
            {
                var newList = new List<FileCabinetRecord>();
                newList.Add(record);
                this.dateOfBirthDictionary.Add(dateOfBirthUpper, newList);
            }

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

            this.list.RemoveAt(id - 1);
            this.list.Insert(id - 1, record);

            string firstNameUpper = firstName.ToUpperInvariant();
            string lastNameUpper = lastName.ToUpperInvariant();
            string dateOfBirthUpper = dateOfBirth.ToString(CultureInfo.InvariantCulture).ToUpperInvariant();

            this.firstNameDictionary[firstNameUpper].RemoveAt(id - 1);
            this.firstNameDictionary[firstNameUpper].Insert(id - 1, record);

            this.lastNameDictionary[lastNameUpper].RemoveAt(id - 1);
            this.lastNameDictionary[lastNameUpper].Insert(id - 1, record);

            this.dateOfBirthDictionary[dateOfBirthUpper].RemoveAt(id - 1);
            this.dateOfBirthDictionary[dateOfBirthUpper].Insert(id - 1, record);
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
                throw new ArgumentException($"{nameof(firstName)} not correct.");
            }

            if (lastName == null || string.IsNullOrEmpty(lastName.Trim()))
            {
                throw new ArgumentNullException($"{nameof(lastName)} cannot be empty.");
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException($"{nameof(lastName)} not correct.");
            }

            DateTime minDate = new DateTime(1950, 1, 1);
            DateTime maxDate = DateTime.Now;

            if (dateOfBirth < minDate || dateOfBirth > maxDate)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} not correct.");
            }

            if (cabinetNumber < 1 || cabinetNumber > 1500)
            {
                throw new ArgumentException($"{nameof(cabinetNumber)} not correct.");
            }

            if (salary < 0 || salary > decimal.MaxValue)
            {
                throw new ArgumentException($"{nameof(salary)} not correct.");
            }

            if (category < 65 || category > 67)
            {
                throw new ArgumentException($"{nameof(category)} not correct.");
            }
        }
    }
}

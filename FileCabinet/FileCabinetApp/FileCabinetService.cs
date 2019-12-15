using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short cabinetNumber, decimal salary, char category)
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

            if (category != 'A' || category != 'B' || category != 'C' ||)
            {
                throw new ArgumentException($"{nameof(category)} not correct.");
            }

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
    }
}

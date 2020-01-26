using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Default printer for the records.
    /// </summary>
    public class DefaultRecordPrinter : IRecordPrinter
    {
        /// <summary>
        /// Prints the list of records to the user.
        /// </summary>
        /// <param name="records">Records to print.</param>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            IEnumerable<FileCabinetRecord> orderedRecords = records.OrderBy(record => record.Id);
            foreach (var record in orderedRecords)
            {
                this.PrintRecord(record);
            }
        }

        /// <summary>
        /// Prints single record.
        /// </summary>
        /// <param name="record">Record to print.</param>
        private void PrintRecord(FileCabinetRecord record)
        {
            var recordString = new StringBuilder();

            recordString.Append($"#{record.Id}, ");
            recordString.Append($"{record.FirstName}, ");
            recordString.Append($"{record.LastName}, ");
            recordString.Append($"{record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, ");
            recordString.Append($"{record.CabinetNumber}, ");
            recordString.Append($"{record.Salary}y.e, ");
            recordString.Append($"{record.Category}");

            Console.WriteLine(recordString);
        }
    }
}

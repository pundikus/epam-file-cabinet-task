using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class implements read records from file in csv-format.
    /// </summary>
    public class FileCabinetRecordCsvReader
    {
        private StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader">Represents characters read.</param>
        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// This Method implements read records from file in csv-format.
        /// </summary>
        /// <returns>List records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            var list = new List<FileCabinetRecord>();
            int position = 70;
            this.reader.BaseStream.Seek(position, SeekOrigin.Begin);

            while (this.reader.Peek() > -1)
            {
                string fileString = this.reader.ReadLine();

                var records = fileString.Split(',');

                var record = new FileCabinetRecord()
                {
                    Id = Convert.ToInt32(records[0], CultureInfo.InvariantCulture),
                    FirstName = records[1],
                    LastName = records[2],
                    DateOfBirth = Convert.ToDateTime(records[3], CultureInfo.InvariantCulture),
                    CabinetNumber = Convert.ToInt16(records[4], CultureInfo.InvariantCulture),
                    Salary = Convert.ToDecimal(records[5], CultureInfo.InvariantCulture),
                    Category = Convert.ToChar(records[6], CultureInfo.InvariantCulture),
                };

                list.Add(record);
            }

            return list;
        }
    }
}

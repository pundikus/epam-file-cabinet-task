﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetGenerator
{
    public class RecordCsvWriter
    {
        private TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Represents characters write.</param>
        public RecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// This Method implements write records in file to csv-format.
        /// </summary>
        /// <param name="record">It is record from our list.</param>
        public void Write(FileCabinetApp.FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var recordString = new StringBuilder();

            recordString.Append($"{record.Id},");
            recordString.Append($"{record.FirstName},");
            recordString.Append($"{record.LastName},");
            recordString.Append($"{record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)},");
            recordString.Append($"{record.CabinetNumber},");
            recordString.Append($"{record.Salary},");
            recordString.Append($"{record.Category}");

            this.writer.WriteLine(recordString.ToString(), Encoding.Default);
        }
    }
}

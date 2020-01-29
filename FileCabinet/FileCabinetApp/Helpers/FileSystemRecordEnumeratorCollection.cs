using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FileCabinetApp.Helpers
{
    /// <summary>
    /// Enumerator for iterating through records.
    /// </summary>
    public class FileSystemRecordEnumeratorCollection : IEnumerable<FileCabinetRecord>
    {
        private FileCabinetFilesystemService service;
        private ReadOnlyCollection<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemRecordEnumeratorCollection"/> class.
        /// </summary>
        /// <param name="service">Service whose records to iterate through.</param>
        /// <param name="records">Records to iterate through.</param>
        public FileSystemRecordEnumeratorCollection(FileCabinetFilesystemService service, ReadOnlyCollection<FileCabinetRecord> records)
        {
            this.service = service;
            this.records = records;
        }

        public static explicit operator List<FileCabinetRecord>(FileSystemRecordEnumeratorCollection enumeratedRecords)
        {
            if (enumeratedRecords == null)
            {
                throw new ArgumentNullException(nameof(enumeratedRecords));
            }

            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            foreach (var record in enumeratedRecords)
            {
                records.Add(record);
            }

            return records;
        }

        /// <summary>
        /// Parsed enumertor to list.
        /// </summary>
        /// <param name="enumeratedRecords">inputs enumerator.</param>
        /// <returns>records to list.</returns>
        public static List<FileCabinetRecord> ToList(FileSystemRecordEnumeratorCollection enumeratedRecords)
        {
            List<FileCabinetRecord> records = (List<FileCabinetRecord>)enumeratedRecords;
            return records;
        }

        /// <summary>
        /// Iterator to iterate through the records.
        /// </summary>
        /// <returns>Single record.</returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            foreach (var record in this.records)
            {
                yield return record;
            }
        }

        /// <summary>
        /// Iterator to iterate through the records.
        /// </summary>
        /// <returns>Single record.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var record in this.records)
            {
                yield return record;
            }
        }
    }
}